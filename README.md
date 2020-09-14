# MySet
Реализация множества, хранящего интервалы целочисленных значений.

`MySet` реализует интерфейс `IEnumerable`

### Пример инициализации множества
* `var set = new MySet("1..4");` Хранит значения 1, 2, 3, 4
* `var set = new MySet("1..4, 6, 8..10");` Хранит значения 1, 2, 3, 4, 6, 8, 9, 10

### Методы класса
* `MySet Union(MySet other)` или `+(MySet other)` - объединение множеств
* `MySet Difference(MySet other)` или `-(MySet other)` - вычитание множеств
* `MySet Intersection(MySet other)` или `*(MySet other)` - пересечение множеств
* `bool Contains(int x)` - проверка нахождения числа 
