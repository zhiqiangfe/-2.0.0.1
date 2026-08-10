# 食品包装产线数字孪生（WPF + HelixToolkit）

这是一个可直接运行的工业 3D 数字孪生教学项目。场景参考典型食品包装线，由程序化轻量模型构成，不包含任何付费 CAD 资产。

## 功能

- WPF 三维产线总览
- 上料输送、视觉检测、三轴拾放机器人、自动封装、出料及缓存区
- 产品沿输送线实时运动
- 机器人和封装头动态动作
- 设备运行、待机、告警、离线状态颜色
- 模拟 PLC 数据：速度、温度、累计处理量、OEE、节拍
- 故障注入和复位
- 点击设备定位三维视角
- 导入 OBJ、STL、3DS、LWO、OFF 网格模型

## 运行

要求 Windows 10/11 和 .NET 9 SDK。

```powershell
cd FactoryTwinDemo
dotnet restore
dotnet run
```

也可以使用 Visual Studio 打开 `FactoryTwinDemo.csproj`。

## 操作

- 左键拖动：旋转视角
- 右键拖动：平移视角
- 鼠标滚轮：缩放
- 点击左侧设备：聚焦设备
- 注入故障：让 PK-401 封装机进入告警并停止产线
- 复位：恢复自动运行
- 导入 CAD：加载 OBJ/STL 等三角网格模型

## 替换成真实 CAD

HelixToolkit 不直接解析 STEP/DWG。推荐先用 FreeCAD、OpenCascade 或 CAD 软件将 STEP 装配体：

1. 删除螺钉、垫片和内部细节；
2. 按可运动设备或部件拆分；
3. 导出为 OBJ/GLB/STL；
4. 在项目中建立 `PLC Tag -> CAD Node -> 状态/Transform` 映射。

STL 不保存装配节点名称，实际数字孪生优先使用 OBJ 或 glTF/GLB。若需要直接支持 STEP，建议增加 OpenCascade 转换服务。

## 接入真实 PLC

当前 `DispatcherTimer` 生成模拟数据。接入真实设备时可增加：

- OPCFoundation.NetStandard.Opc.Ua
- MQTTnet
- S7.NetPlus

把 PLC 数据统一转换为设备状态对象，再由 UI 线程批量刷新颜色和 Transform。不要在 OPC 回调线程直接操作 WPF 三维对象。

## 代码入口

- `MainWindow.xaml`：工业监控界面
- `MainWindow.xaml.cs`：三维场景、动画、CAD 导入和仿真
- `Models/EquipmentItem.cs`：设备孪生状态模型

## 许可证说明

项目中的三维场景为教学用途原创程序化模型。HelixToolkit.Wpf 使用其 MIT 许可证。
