在创建角色时，会有不同的职业，每个职业都有不同的能力。
一般的写法是创建一个基类BaseActor，所有的职业都继承这个基类。然后在各个职业的类中写自己的能力。

弊端：随着能力的增多，很可能出现某个能力A职业需要，B职业也需要的情况，那么就只能复制A的能力到B。这不但增加了冗余代码，还非常难以维护，如果出现问题就只能一个个的排查

这时候可以依照依赖倒置的思想优化代码。依赖倒置其实就是要求我们不要依赖具体，而要依赖抽象。本质上就是面向接口编程。
这个例子中，每个职业都有的具体的能力让我们维护很困难。那么就可以将能力抽象出来。所有的能力都可以抽象成一种行为。那么创建一个接口IAction来代表所有的能力。
在BaseActor中只保留一个Dictionary来存放所有的行为，即IAction。IAction需要提供一个方法Execute，由实现IAction的类实现（多态），这样BaseActor只需要调用每个行为的Execute即可，而不需要关心这个行为是哪一个
BaseActove在初始化的时候就可以实例化各种具体的能力添加到Dictionary中。继承自BaseActor的所有职业都如此，这样相同的能力就是实现了IAction的类的对象，没有了冗余的代码，出问题时也只需要排查这个类。
此外，BaseActor还可以增加一个添加行为的方法，方便在游戏中增加能力。

如果职业的能力有很多，例如有攻击、移动、跳跃、飞行等等能力。而这些能力又有不同，攻击有普通攻击，特殊攻击，特殊攻击2等等，移动有普通移动、特殊移动、高级移动等等。。。
他们之间又需要扩展，可能出现冗余代码，那么可以将这些再抽象出一层，攻击IAttack，移动IMove，跳跃IJump，飞行IFly，这些接口都继承IAction，那么各种细分能力只需要实现对应的接口即可。
普通攻击，特殊攻击，特殊攻击2都实现IAttack，由于IAttack都继承了IAction，因此都可以放进BaseActor中，也会实现Execute方法。