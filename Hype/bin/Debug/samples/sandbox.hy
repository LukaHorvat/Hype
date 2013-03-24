Range = type
{
	start = 0;
	end = 0;
	step = 0;
};

asd = new Range;

printLine "test";

range = function [Number start, Number end, Number step]
{
	printLine "asd";
	r = new Range;
	r.start = start;
	r.end = end;
	r.step = step;
	return r;
};
printLine "test";

printLine "break";
forRange = function [Function iter, CodeBlock block]
{
	for {}{iter block}{}{};
};
printLine "break";

in = function [i, Range r]
{
	delta = r.step;
	i = r.start - r.step;
	return (function [CodeBlock block]
	{
		i = i + r.step;
		if (i <= r.end)
		{
			exec block;
		};
	});
};
printLine "test";

a = range 0 10 2;
printLine "test";
printLine a;