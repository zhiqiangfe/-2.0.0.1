# Codex 开发聊天备份

更新时间：2026-07-13

## 项目信息

- 项目路径：`D:\安装包\2.0.0.1\2.0.0.1`
- 解决方案：`HTHIUM.sln`
- 当前主项目名：`HTHIUM`
- 当前 Git 远端：`git@github.com:zhiqiangfe/-2.0.0.1.git`
- 当前分支：`master`
- 最近关键提交：`a44f417 Add HMI alarm database monitoring`

## 已完成的主要工作

### 1. 项目品牌替换

已将原项目中的 `SUNWODA_SEVB` 相关命名整体替换为 `HTHIUM`，并保留数据库连接相关配置，避免因数据库名或连接串变化导致程序无法启动。

注意事项：

- 数据库连接配置暂未强制改名。
- 如果后续继续品牌清理，需要重点检查程序集名、XAML 命名空间、bin/obj 缓存、数据库配置、版本文件。

### 2. 登录页优化

登录页已改为 HTHIUM 设备智慧系统风格，包含：

- 左侧工业蓝图视觉区域
- HTHIUM 标识
- 设备智慧系统主标题
- 右侧登录面板
- 图片形式的智能助手视觉
- 密码输入相关控件修复

曾出现过 `HTHIUM.Component.dll, Version=2.1.0.0` 找不到的问题，后续已统一切回自动版本模式，避免手动写死版本号造成 DLL 加载问题。

### 3. 智慧管理页面

已在 SmartManagement 下陆续设计并实现多个 WPF 页面，数据先使用模拟数据或数据库演示数据：

- 设备 OEE 分析
- 工位 CT 与动作节拍瓶颈分析
- HMI 报警记录与报警 Top 分析
- 报警触发时 PLC 状态快照追溯
- 产品品质追溯
- CPK / 参数相关性分析
- 伺服轴健康监控
- 气缸监控
- 过程参数监控，以激光器功率曲线还原焊接过程为例

页面设计注意事项：

- 尽量避免整页滚动。
- 标题栏需要压缩高度，避免遮挡小标题。
- ComboBox、DateTimePicker、摘要卡片等控件要检查文字是否被遮挡。
- 报警索引、HMI 报警列表、PLC 快照列表等需要支持选中刷新右侧详情。

### 4. HMI 报警数据库设计

当前报警页面重点落地两个表：

#### hmi_alarm_code_map

用途：报警代码关联表，用于根据 PLC/HMI 读取到的报警代码查询报警名称、等级、可能原因和处理建议。

核心字段：

- `alarm_code`：报警代码
- `alarm_name`：报警名称
- `alarm_level`：报警等级
- `possible_reason`：报警可能原因
- `handle_suggestion`：处理建议
- `device_name`：设备名称
- `station_name`：工位名称
- `process_name`：工序名称
- `is_enable`：是否启用

对应模型：

- `HTHIUM.Data\Models\HmiAlarmCodeMap.cs`

#### hmi_alarm_record

用途：报警记录表，保存报警从触发到恢复的完整生命周期。

核心字段：

- `line_name`：产线名称
- `device_name`：设备名称
- `station_name`：工位名称
- `process_name`：工序名称
- `alarm_code`：报警代码
- `alarm_name`：报警名称
- `alarm_level`：报警等级
- `trigger_time`：报警开始时间
- `recover_time`：报警恢复时间
- `duration_seconds`：持续秒数
- `alarm_status`：报警状态，例如 `触发中`、`已恢复`
- `source`：来源，例如 `PLC`
- `raw_value`：原始点位或参数名
- `created_time`：创建时间
- `updated_time`：更新时间

对应模型：

- `HTHIUM.Data\Models\HmiAlarmRecord.cs`

初始化演示数据位置：

- `HTHIUM.Data\DatabaseInitializer.cs`

### 5. HMI 报警页面接入数据库

页面：

- `HTHIUM\Views\Pages\SmartManagement\HmiAlarmTopAnalysisPage.xaml`

ViewModel：

- `HTHIUM\ViewModels\Pages\SmartManagement\VM_HmiAlarmTopAnalysisPage.cs`

已完成逻辑：

- 按时间范围查询 `hmi_alarm_record`
- 时间控件使用 HandyControl 的 `DateTimePicker`
- 报警 Top 按 `alarm_code + alarm_name` 分组统计
- 报警详情随左侧列表选中项刷新
- 报警 Top 和分布矩阵按时间范围统计，不随选中项改变
- 如果时间范围内没有数据库记录，显示空数据状态，不再自动 fallback 到模拟数据

### 6. PLC 报警后台采集逻辑

新增后台服务：

- `HTHIUM\Services\SmartManagement\HmiAlarmMonitorHostedService.cs`

注册位置：

- `HTHIUM\App.xaml.cs`

注册方式：

```csharp
services.AddHostedService<HmiAlarmMonitorHostedService>();
```

后台采集逻辑：

1. 程序启动后由 HostedService 自动运行，不需要导航到报警页面才运行。
2. 后台循环读取 `global_setting` 中的配置。
3. 等待 PLC 服务初始化完成。
4. 根据 `plc_address_config.parameter_name` 找到：
   - `报警触发地址`
   - `报警代码`
5. 当报警触发信号有效，且报警代码不为空、不为 `0`：
   - 根据报警代码查询 `hmi_alarm_code_map`
   - 生成一条 `hmi_alarm_record`
   - 写入 `TriggerTime = DateTime.Now`
   - 写入 `AlarmStatus = 触发中`
   - 保存插入后的记录 ID 到内存 `_activeRecordId`
6. 当报警触发信号回到 `0`：
   - 根据 `_activeRecordId` 找到同一条报警记录
   - 写入 `RecoverTime = DateTime.Now`
   - 计算 `DurationSeconds`
   - 更新 `AlarmStatus = 已恢复`

这样可以保证报警开始时间和恢复时间写入同一条记录。

边界情况：

- 如果程序在报警触发中重启，内存中的 `_activeRecordId` 会丢失。
- 当前逻辑有一定数据库兜底查询，但正式生产建议启动时扫描所有 `触发中` 报警，并结合 PLC 当前状态做恢复补偿。

### 7. global_setting 配置项

已新增或使用的关键配置：

- `IsHmiAlarmMonitorEnabled`：是否启用 HMI 报警后台采集
- `HmiAlarmMonitorCycleMs`：报警后台采集周期，默认 `1000`
- `HmiAlarmActiveParameterName`：报警触发信号参数名，默认 `报警触发地址`
- `HmiAlarmCodeParameterName`：报警代码参数名，默认 `报警代码`
- `LineName`：当前产线名称，默认 `L1 密封钉线`

报警记录里的 `LineName` 读取方式：

1. `HmiAlarmMonitorHostedService` 每轮从依赖注入容器创建 scope。
2. 从 scope 中获取 `IGlobalSettingRepository`。
3. 调用：

```csharp
await globalSettingRepo.GetSettingValueAsync<string>("LineName", "-");
```

4. 把读到的值传入报警处理函数。
5. 插入 `HmiAlarmRecord` 时写入 `LineName` 字段。

### 8. PLC 字符串高低字节处理

问题现象：

数据库配置：

- `type = string`
- `address = D106`
- `length = 15`

PLC 实际报警代码：

```text
HMI-SENSOR-066
```

程序原始读到：

```text
MH-IESSNRO0-66\0\0
```

原因：

- 一个 D 寄存器是 2 个字节。
- PLC 存储字符串时高低字节顺序和程序解析顺序相反。

修复位置：

- `HTHIUM.PLC\PLCService.cs`

修复方式：

- `STRING` 类型读取后，每 2 个字符交换一次。
- 裁剪末尾 `\0` 和空格。

效果：

```text
MH-IESSNRO0-66\0\0
```

会转换为：

```text
HMI-SENSOR-066
```

### 9. Git 上传记录

本地提交：

```text
a44f417 Add HMI alarm database monitoring
```

推送远端：

```text
git@github.com:zhiqiangfe/-2.0.0.1.git
```

普通 SSH 22 端口曾被中断，最终使用 GitHub SSH over HTTPS 方式成功推送：

```powershell
$env:GIT_SSH_COMMAND='ssh -p 443 -o HostName=ssh.github.com -o StrictHostKeyChecking=accept-new'
git push backup-new master
```

另一台电脑如果 22 端口不通，也可以使用同样方式推送。

## 常用命令

### 编译项目

```powershell
dotnet build "D:\安装包\2.0.0.1\2.0.0.1\HTHIUM.sln" --configuration Debug -v:minimal -m:1 /nr:false /p:UseSharedCompilation=false
```

### 克隆仓库

```powershell
git clone git@github.com:zhiqiangfe/-2.0.0.1.git
```

### 推送仓库

```powershell
git push backup-new master
```

如果 22 端口不通：

```powershell
$env:GIT_SSH_COMMAND='ssh -p 443 -o HostName=ssh.github.com -o StrictHostKeyChecking=accept-new'
git push backup-new master
```

## 下一步建议

1. 报警采集逻辑增加“启动补偿”，处理程序重启时仍处于 `触发中` 的报警。
2. `plc_address_config` 可以继续通过 `parameter_name` 管理业务点位，但长期建议增加业务分类字段，例如报警、生产、过程参数、状态。
3. 报警记录量大后，页面左侧报警索引建议改成分页或按时间检索，不要一次加载几百上千条。
4. 报警代码建议和 PLC 约定统一格式，例如直接使用 `HMI-SENSOR-066`，避免程序再做二次映射。
5. 后续真实数据接入后，OEE、CT、CPK、过程参数曲线等页面需要从模拟数据切换为数据库查询或实时采集服务。
