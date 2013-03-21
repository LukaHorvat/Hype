Animal = type
{
	name = "cat";
	setName = function [String n]
	{
		name = n;
	};
};

cat = new Animal;
dog = new Animal;
dog.setName "dog";
dog.asdf = "New field";
printLine cat.name;
printLine dog.name;
printLine dog.asdf;