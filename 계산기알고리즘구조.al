struct 식[]
float[] 피연산자 1, 피연산자 2, 버퍼 	(알고리즘에선 '스택'로 간주)
string[] 연산자		  	(알고리즘에선 '큐'으로 간주)

괄호가 한개 추가되는것이 식 배열의 인덱스가 1 늘어나는 것과 같음.

Screen : 계산결과가 출력될 라벨
ScreenBuffer : 피연산자가 입력될 공간

연산자를 입력하는 것이 피연산자의 '항 입력'이 끝났다는 것을 의미함.
연산자를 입력했을때 enqueue -> TryCalc -> push -> deletebuffer의 순서대로 일어난다.(ETPD)

[enqueue]
- 큐가 꽉차있을경우 버퍼에 피연산자를 넣는다.
- 피연산자를 큐에 넣는다. (피연산자를 enqueue할땐 string(ScreenBuffer)에서 float으로 형변환.)

[enqueue(숫자)]
- 숫자를 큐에 넣는다.

[TryCalc]
- 연산이 유효한지 확인한다.(나머지연산을 하려는데, 소수가 있을경우) 유효하지 않으면 error.
- 큐가 꽉차지 않았을경우 return
elseif 스택에 연산자가 없을경우 return						(case1)
elseif 스택에 연산자가 1개있고 그 연산자가 *, / 인경우			
ㄴ 스택에서 pop 하여 피연산자와 연산한다. 						(case2)
elseif 스택에 연산자가 1개있고 그 연산자가 +, - 인경우	
ㄴ push하려는 연산자가 +, - 인경우 pop하여 피연산자와 연산한다.			(case3)
ㄴ push하려는 연산자가 *, / 인경우 return					(case4)
elseif 스택에 연산자가 2개있고, push하려는 연산자가 *, / 인경우		
ㄴ 스택을 pop하여 버퍼와 피연산자2를 연산하여 피연산자2 노드에 연산결과를 대입한다.		(case5)
	
ㄴ Top이 +, - 일경우 스택이 빌 때까지 pop 하여 큐에있는 피연산자와 연산한다. goto init.	
		
(init) InitStruct를 호출한다.
(print) 연산결과를 Screen에 출력하고 enqueue(연산결과)를 호출한다.

[push]
- 연산자를 푸시한다.

[deletebuffer]
- ScreenBuffer 내용을 지운다.

[InitStruct]
- 스택과 큐를 비운다.

[ReturnResult('=' 눌렀을때)]
- 연산자가 입력되지 않았을 경우 return
elseif 피연산자가 0개인 경우 return
elseif 피연산자가 1개일 경우 (Screen, 연산자, 피연산자)의 연산을 한다.
else(피연산자가 2개일 경우) TryCalc를 호출한다.
- 연산결과가 나온경우 연산결과의 IsDot여부(소수인지 자연수인지)를 판단한다.

[아직 제대로 생각하지 않은것]

단항연산자(피연산자를 바로 변환할 예정)
맨처음 or 괄호열고 처음 쓰는 '-' 연산자는 (isnegative로 할 예정)