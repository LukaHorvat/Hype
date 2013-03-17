Animal = type
{
	name = "cat";
};

cat = new Animal;
dog = new Animal;
dog.name = "dog";
printLine cat.name;
printLine dog.name;