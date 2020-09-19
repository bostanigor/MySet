# IntervalTree
Реализация дерева поиска интервалов. Информация о структуре данных взята из следующих источников:
 * https://en.wikipedia.org/wiki/Interval_tree
 * https://neerc.ifmo.ru/wiki/index.php?title=%D0%94%D0%B5%D1%80%D0%B5%D0%B2%D0%BE_%D0%B8%D0%BD%D1%82%D0%B5%D1%80%D0%B2%D0%B0%D0%BB%D0%BE%D0%B2_(interval_tree)_%D0%B8_%D0%BF%D0%B5%D1%80%D0%B5%D1%81%D0%B5%D1%87%D0%B5%D0%BD%D0%B8%D0%B5_%D1%82%D0%BE%D1%87%D0%BA%D0%B8_%D1%81_%D0%BC%D0%BD%D0%BE%D0%B6%D0%B5%D1%81%D1%82%D0%B2%D0%BE%D0%BC_%D0%B8%D0%BD%D1%82%D0%B5%D1%80%D0%B2%D0%B0%D0%BB%D0%BE%D0%B2

В IntervalTree метод GetEnumerator(), позволяющий обращаться к нему, как к колллекции.

### Пример инициализации IntervalTree
`
var tree = new IntervalTree(new List<MyRange>{
                    new MyRange(0, 10), 
                    new MyRange(13, 15), 
                    new MyRange(17, 20)
                }
            );
var tree = new IntervalTree(new List<MyRange>{
                    new MyRange(0, 10), 
                    new MyRange(5, 15), 
                    new MyRange(17, 20)
                }
            );
`
### Реализация
IntervalTree содержит `TreeNode root`.
  
`TreeNode` содержит медиану отрезка, левую и правую границы отрезка, ссылки на левого и правого потомков.

При инициализации `IntervalTree` интервалы сортируются по левой границе, а пересекающиеся интервалы объединяются.

### Методы класса
* `IEnumerator<int> GetEnumerator()` - для реализации `foreach`
* `bool Contains(int x)` - проверка на наличие элемента в дереве. `O(logN)`

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

### Реализация
MySet хранит 2 массива границ интервалов - `_lefts` и `_rights`. Таким образом количество занимаемой памяти зависит от количества интервалов при инициализации.
