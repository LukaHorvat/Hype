repeatN = function [Number n, CodeBlock block]
{
	for {i = 0}{i < n}{i = i + 1} block;
};

repeat5 = repeatN 5;
repeat5 { printLine i; };
printLine i;