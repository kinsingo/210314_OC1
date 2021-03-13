#pragma once
#include "stdafx.h"

class State
{
public:
	virtual void handle() = 0;
};


class ConcreteState1 : public State
{
public:
	void handle() override;
};

class ConcreteState2 : public State
{
public:
	void handle() override;
};

class Context
{
private:
	int* A;
	State* pState;
	ConcreteState1* concreteState1;
	ConcreteState2* concreteState2;
};

