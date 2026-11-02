; Sun Tuner — PvZ Hybrid Remake 阳光频率调节模组
; Godot 4.x GDScript Autoload
; 作者: minizyz

extends Node

# ====== 配置 ======
const CFG_PATH := "user://sun_tuner.cfg"
const DEFAULT_SKY_INTERVAL := 5.0
const DEFAULT_SKY_AMOUNT := 25
const DEFAULT_SUNFLOWER_INTERVAL := 12.0
const DEFAULT_SUNFLOWER_MULT := 1.0

# 节点名/类名嗅探关键词（按常见命名习惯）
const SKY_SUN_KEYWORDS := ["sun_spawn", "skysun", "sun_drop", "sundrop", "sky_sun", "weather", "sun_manager"]
const SUNFLOWER_KEYWORDS := ["sunflower", "向日葵", "sun_producer", "sunproducer"]

# ====== 运行时状态 ======
var cfg_enabled := true
var sky_interval := DEFAULT_SKY_INTERVAL
var sky_amount := DEFAULT_SKY_AMOUNT
var sunflower_interval := DEFAULT_SUNFLOWER_INTERVAL
var sunflower_mult := DEFAULT_SUNFLOWER_MULT

var patched_sky_timers: Array[Timer] = []
var patched_sunflowers: Array[Node] = []
var config_dialog: Control = null


func _ready() -> void:
	load_config()
	print("[SunTuner] 加载完成. 天空间隔=%.1fs 向日葵间隔=%.1fs" % [sky_interval, sunflower_interval])
	# 延迟一帧，等游戏主场景加载完
	await get_tree().process_frame
	auto_patch_all()
	build_ui()
	# 监听场景切换，新关卡加载后重新 patch
	get_tree().node_added.connect(_on_node_added)


func load_config() -> void:
	var cfg := ConfigFile.new()
	var err := cfg.load(CFG_PATH)
	if err != OK:
		print("[SunTuner] 未找到配置文件，使用默认值: ", CFG_PATH)
		return
	cfg_enabled = cfg.get_value("general", "enabled", true)
	sky_interval = float(cfg.get_value("sky_sun", "interval", DEFAULT_SKY_INTERVAL))
	sky_amount = int(cfg.get_value("sky_sun", "amount", DEFAULT_SKY_AMOUNT))
	sunflower_interval = float(cfg.get_value("sunflower", "interval", DEFAULT_SUNFLOWER_INTERVAL))
	sunflower_mult = float(cfg.get_value("sunflower", "amount_multiplier", DEFAULT_SUNFLOWER_MULT))


func save_config() -> void:
	var cfg := ConfigFile.new()
	cfg.set_value("general", "enabled", cfg_enabled)
	cfg.set_value("sky_sun", "interval", sky_interval)
	cfg.set_value("sky_sun", "amount", sky_amount)
	cfg.set_value("sunflower", "interval", sunflower_interval)
	cfg.set_value("sunflower", "amount_multiplier", sunflower_mult)
	cfg.save(CFG_PATH)
	print("[SunTuner] 配置已保存到 ", CFG_PATH)


# ====== 节点嗅探与 patch ======

func auto_patch_all() -> void:
	if not cfg_enabled:
		return
	patched_sky_timers.clear()
	patched_sunflowers.clear()
	_walk_tree(get_tree().root)
	print("[SunTuner] 已 patch %d 个天空阳光定时器, %d 个向日葵" % [patched_sky_timers.size(), patched_sunflowers.size()])


func _walk_tree(node: Node) -> void:
	if _is_sky_sun_spawner(node):
		_patch_sky_sun(node)
	elif _is_sunflower(node):
		_patch_sunflower(node)
	for child in node.get_children():
		_walk_tree(child)


func _on_node_added(node: Node) -> void:
	await get_tree().process_frame
	if _is_sky_sun_spawner(node):
		_patch_sky_sun(node)
	elif _is_sunflower(node):
		_patch_sunflower(node)


func _is_sky_sun_spawner(node: Node) -> bool:
	var name_lower := node.name.to_lower()
	for kw in SKY_SUN_KEYWORDS:
		if kw.to_lower() in name_lower:
			return true
	if node.get_script() is GDScript:
		var script: GDScript = node.get_script()
		if script.resource_path != "":
			for kw in SKY_SUN_KEYWORDS:
				if kw.to_lower() in script.resource_path.to_lower():
					return true
	return false


func _is_sunflower(node: Node) -> bool:
	var name_lower := node.name.to_lower()
	for kw in SUNFLOWER_KEYWORDS:
		if kw.to_lower() in name_lower:
			return true
	if node.get_script() is GDScript:
		var script: GDScript = node.get_script()
		if script.resource_path != "":
			for kw in SUNFLOWER_KEYWORDS:
				if kw.to_lower() in script.resource_path.to_lower():
					return true
	return false


func _find_timer(parent: Node) -> Timer:
	for child in parent.get_children():
		if child is Timer:
			return child
		if child.name.to_lower().contains("timer"):
			var found := _find_timer(child)
			if found:
				return found
	return null


func _patch_sky_sun(spawner: Node) -> void:
	var timer := _find_timer(spawner)
	if timer == null:
		return
	if timer in patched_sky_timers:
		return
	timer.wait_time = sky_interval
	timer.start(sky_interval)
	patched_sky_timers.append(timer)
	if "sun_amount" in spawner:
		spawner.set("sun_amount", sky_amount)
	elif "amount" in spawner:
		spawner.set("amount", sky_amount)


func _patch_sunflower(plant: Node) -> void:
	if plant in patched_sunflowers:
		return
	var timer := _find_timer(plant)
	if timer:
		timer.wait_time = sunflower_interval
		timer.start(sunflower_interval)
	patched_sunflowers.append(plant)
	if "sun_amount" in plant:
		var orig: int = plant.get("sun_amount")
		plant.set("sun_amount", int(orig * sunflower_mult))


# ====== 配置 UI（屏幕角落小按钮 + 弹窗）======

func build_ui() -> void:
	var vp := get_tree().root.get_visible_rect().size
	var btn_size := Vector2(64, 64)
	var margin := 16.0
	var pos := Vector2(vp.x - btn_size.x - margin, margin)

	var btn := Button.new()
	btn.text = "\U0001F33B"
	btn.custom_minimum_size = btn_size
	btn.position = pos
	btn.pressed.connect(_on_open_config)
	get_tree().root.add_child(btn)


func _on_open_config() -> void:
	if config_dialog and is_instance_valid(config_dialog):
		config_dialog.queue_free()
	config_dialog = _build_dialog()
	get_tree().root.add_child(config_dialog)


func _build_dialog() -> Control:
	var vp := get_tree().root.get_visible_rect().size
	var dialog := PanelContainer.new()
	dialog.position = Vector2(vp.x * 0.1, vp.y * 0.1)
	dialog.custom_minimum_size = Vector2(vp.x * 0.8, vp.y * 0.7)

	var vbox := VBoxContainer.new()
	vbox.add_theme_constant_override("separation", 12)
	vbox.offset_left = 16
	vbox.offset_right = -16
	vbox.offset_top = 16
	vbox.offset_bottom = -16

	var title := Label.new()
	title.text = "\U0001F33B Sun Tuner 控制面板"
	title.add_theme_font_size_override("font_size", 22)
	vbox.add_child(title)

	var enable_row := HBoxContainer.new()
	var enable_label := Label.new()
	enable_label.text = "模组启用"
	var enable_btn := Button.new()
	enable_btn.text = "ON" if cfg_enabled else "OFF"
	enable_btn.pressed.connect(func():
		cfg_enabled = not cfg_enabled
		enable_btn.text = "ON" if cfg_enabled else "OFF"
		if cfg_enabled:
			auto_patch_all()
		save_config()
	)
	enable_row.add_child(enable_label)
	enable_row.add_child(enable_btn)
	vbox.add_child(enable_row)

	vbox.add_child(_make_slider("天空阳光间隔（秒）", sky_interval, 1.0, 60.0, func(v: float):
		sky_interval = v
		for t in patched_sky_timers:
			if is_instance_valid(t):
				t.wait_time = v
		save_config()
	))

	vbox.add_child(_make_slider("天空阳光数量", float(sky_amount), 5.0, 500.0, func(v: float):
		sky_amount = int(v)
	))

	vbox.add_child(_make_slider("向日葵产光间隔（秒）", sunflower_interval, 1.0, 120.0, func(v: float):
		sunflower_interval = v
		for p in patched_sunflowers:
			if is_instance_valid(p):
				var t := _find_timer(p)
				if t:
					t.wait_time = v
		save_config()
	))

	vbox.add_child(_make_slider("向日葵数量倍率", sunflower_mult, 0.1, 5.0, func(v: float):
		sunflower_mult = v
		save_config()
	))

	var preset_row := HBoxContainer.new()
	preset_row.add_theme_constant_override("separation", 8)
	var presets := [
		["原版", 10.0, 25, 24.0, 1.0],
		["轻松", 5.0, 25, 12.0, 1.0],
		["爽玩", 1.0, 50, 3.0, 2.0],
		["硬核", 20.0, 15, 48.0, 0.5],
	]
	for p in presets:
		var b := Button.new()
		b.text = p[0]
		b.pressed.connect(func():
			sky_interval = p[1]
			sky_amount = p[2]
			sunflower_interval = p[3]
			sunflower_mult = p[4]
			auto_patch_all()
			save_config()
			if config_dialog and is_instance_valid(config_dialog):
				config_dialog.queue_free()
			_on_open_config()
		)
		preset_row.add_child(b)
	vbox.add_child(preset_row)

	var close := Button.new()
	close.text = "关闭"
	close.pressed.connect(func(): dialog.queue_free())
	vbox.add_child(close)

	dialog.add_child(vbox)
	return dialog


func _make_slider(label_text: String, value: float, min_v: float, max_v: float, on_changed: Callable) -> HBoxContainer:
	var row := HBoxContainer.new()
	row.add_theme_constant_override("separation", 8)

	var label := Label.new()
	label.text = label_text
	label.custom_minimum_size = Vector2(200, 0)

	var slider := HSlider.new()
	slider.min_value = min_v
	slider.max_value = max_v
	slider.step = 0.5 if max_v <= 10 else 1.0
	slider.value = value
	slider.custom_minimum_size = Vector2(200, 0)

	var val_label := Label.new()
	val_label.text = "%.1f" % value
	val_label.custom_minimum_size = Vector2(60, 0)

	slider.value_changed.connect(func(v: float):
		val_label.text = "%.1f" % v
		on_changed.call(v)
	)

	row.add_child(label)
	row.add_child(slider)
	row.add_child(val_label)
	return row
