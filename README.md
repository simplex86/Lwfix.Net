# Lwfix.Net

Lwfix.Net 是基于 .Net 实现的跨平台有符号定点数库，包含以下几个模块

- 定点数
	- Fixed32：Q32.32 格式
	- Fixed64：Q64.64 格式
	- 数值计算：最大值、最小值、绝对值、幂等常用函数
	- 三角函数：正弦、余弦、正切及其反函数
- 向量
	- FVector2：二维向量
	- FVector3：三维向量
- 矩阵
	- FMatrix3x3：3x3矩阵
	- FMatrix4x4：4x4矩阵
- 四元数：FQuaternion
- 随机数：FRandom

> [!WARNING]  
> Fixed64 需要在 .Net 7 及更高版本才被支持

详情请看 [参考手册](./Doc/README.md)

> 我本人原来是 Unity3D 前端程序员，这个库最初也是为方便游戏开发而写的，所以大部分函数接口和 Unit3D 的接口相同（或相似）