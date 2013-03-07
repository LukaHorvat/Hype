add2 = function [Number n]
{
	n + 2;
};

add3 = function [Number n]
{
	add2 n + 1;
};

addMTimes3 = function [Number n, Number m]
{
	for {i = 0}{i < m}{i = i + 1}
	{
		n = add3 n; 
	};
	n;
};

a = (addMTimes3 5 10);