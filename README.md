# SUNWODA_SEVB.Data

## 一、描述

数据库

## 二、数据库表名称变化

|      旧      |        新         |
| :----------: | :---------------: |
|    device    | device_production |
|   loglevel   |     log_level     |
|   probably   | production_status |
|     role     |     user_role     |
|    users     |    users_info     |
|  valuetypes  |    value_types    |
| variabletype |   variable_type   |

## 三、线程管理

**HostedService vs ThreadManager 对比分析**

| 方面             | HostedService                     | ThreadManager                 |
| ---------------- | --------------------------------- | ----------------------------- |
| **生命周期管理** | 由.NET Host自动管理，支持优雅关闭 | 需手动管理启动/停止，容易泄露 |
| **异步支持**     | 原生支持 async/await              | 需自行实现取消机制            |
| **异常处理**     | 内置异常处理和重启机制            | 需要手动处理                  |
| **资源清理**     | 自动释放资源                      | 手动清理，容易遗漏            |
| **资源消耗**     | 基于 Task 的轻量级调度            | 每个线程占用 1MB+ 内存        |
| **取消令牌**     | 内置CancellationToken支持         | 自定义信号机制                |
| **日志集成**     | 完美集成.NET日志系统              | 需要手动集成                  |
| **依赖注入**     | 原生支持DI                        | 静态实现，难以集成DI          |
| **测试友好**     | 易于单元测试                      | 静态依赖，难以测试            |
| **现代化程度**   | .NET推荐的现代方式                | 传统线程管理方式              |

**推荐使用HostedService的理由：**

1. **更好的生命周期管理**：自动启动/停止
2. **异常恢复**：服务异常时可以自动重启
3. **优雅关闭**：支持应用关闭时的优雅处理
4. **更好的可测试性**：可以轻松进行单元测试
5. **符合.NET最佳实践**：微软推荐的后台服务模式
