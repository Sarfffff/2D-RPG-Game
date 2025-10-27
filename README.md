2D-RPG-Game

这是一个基于 Unity 开发的 2D 角色扮演游戏（RPG）项目。

项目信息

- 开发引擎：Unity 2022.3.2t17
- 核心功能：2D 角色扮演游戏基础框架，包含角色、敌人、物品、战斗等核心 RPG 元素

项目结构

    2D-RPG-Game/
    ├── Assets/                  # 游戏资源目录
    │   ├── Animator/            # 动画控制器和动画片段
    │   ├── Data/                # 游戏数据（物品、装备等）
    │   ├── Graphics/            # 图形资源（精灵、特效、场景元素等）
    │   ├── Script/              # 游戏脚本
    │   ├── TextMesh Pro/        # TextMesh Pro文本组件资源
    │   ├── Tile Palette/        # 瓦片地图资源
    │   └── Scenes/              # 游戏场景
    ├── Packages/                # 项目依赖包配置
    └── ProjectSettings/         # Unity项目设置

核心功能模块

1. 角色系统：包含角色属性、状态管理
2. 敌人系统：实现多种敌人（Slime、Skeleton 等）的 AI 和行为
3. 物品系统：装备、道具等物品的定义和管理
4. 战斗系统：包含攻击、技能、特效等战斗相关逻辑
5. UI 系统：游戏界面相关组件和逻辑

依赖项

项目使用了以下主要 Unity 包：

- com.unity.cinemachine (2.9.7)
- com.unity.feature.2d (2.0.0)
- com.unity.textmeshpro (3.0.9)
- com.unity.ugui (1.0.0)
- 以及其他 Unity 官方模块

快速开始

1. 确保安装了 Unity 2022.3.2t17 或兼容版本
2. 克隆或下载项目到本地
3. 打开 Unity Hub，点击 "添加" 按钮，选择项目根目录
4. 等待项目加载完成后，打开Assets/Scenes/MainMenu.scene即可开始

注意事项

- 项目使用.gitignore 忽略了 Unity 生成的临时文件、构建产物等，确保版本控制的整洁性
- TextMesh Pro 资源包含字体和样式表，用于游戏中的文本显示
- 敌人动画和特效资源位于Graphics目录下，可根据需求扩展
