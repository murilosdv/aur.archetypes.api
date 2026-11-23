¨	
JD:\git\aur.archetypes.api\src\App.Api\Resources\Shared\CallerExtensions.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Shared2 8
;8 9
internal 
static	 
class 
CallerExtensions &
{		 
public

 

static

 
Caller

 
ToCaller

 !
(

! "
this

" &
ClaimsPrincipal

' 6
user

7 ;
)

; <
{ 
var 
subject 
= 
user 
. 
	GetUserId $
($ %
)% &
?? 
throw 
new %
InvalidOperationException 2
(2 3
$str3 x
)x y
;y z
return 
new 
Caller 
( 
subject !
,! "
user# '
.' (
IsInRole( 0
(0 1
$str1 8
)8 9
)9 :
;: ;
} 
} ﬁ
FD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\PostsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
;7 8
internal 
sealed	 
class 
PostsResource #
:$ %
ResourceGroup& 3
;3 4∂
VD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\PublishPostEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
class 
PublishPostEndpoint )
:* +
MinimalEndpoint, ;
<; <
PostsResource< I
>I J
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapPost 
( 
$str (
,( )
Handle* 0
)0 1
. 
RequireAnyRole 
( 
$str $
,$ %
$str& -
)- .
. 
RequireScopes 
( 
	AppScopes $
.$ %
Write% *
)* +
;+ ,
static 
async 
Task 
< 
Results !
<! "
Ok" $
<$ %
PostResponse% 1
>1 2
,2 3
ProblemHttpResult4 E
>E F
>F G
HandleH N
(N O
Guid 
id 
, 
ClaimsPrincipal 
user  
,  !
PublishPost 
feature 
,  
CancellationToken 
cancellationToken /
)/ 0
{   	
var!! 
result!! 
=!! 
await!! 
feature!! &
.!!& '
HandleAsync!!' 2
(!!2 3
new!!3 6
PublishPostRequest!!7 I
(!!I J
id!!J L
,!!L M
user!!N R
.!!R S
ToCaller!!S [
(!![ \
)!!\ ]
)!!] ^
,!!^ _
cancellationToken!!` q
)!!q r
;!!r s
return## 
result## 
.## 
ToHttpResult## &
(##& '
)##' (
;##( )
}$$ 	
}%% 
}&& à%
TD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\PatchPostEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
record 
PatchPostBody $
($ %
string% +
?+ ,
Title- 2
,2 3
string4 :
?: ;
Body< @
)@ A
;A B
internal 
sealed	 
class "
PatchPostBodyValidator ,
:- .
RequestValidator/ ?
<? @
PatchPostBody@ M
>M N
{ 
public 
"
PatchPostBodyValidator !
(! "
)" #
{ 
RuleFor 
( 
x 
=> 
x 
) 
. 
Must 
( 
x 
=> !
x" #
.# $
Title$ )
is* ,
not- 0
null1 5
||6 8
x9 :
.: ;
Body; ?
is@ B
notC F
nullG K
)K L
.L M
WithMessageM X
(X Y
$strY |
)| }
;} ~
RuleFor 
( 
x 
=> 
x 
. 
Title 
) 
. 
NotEmpty &
(& '
)' (
.( )
MaximumLength) 6
(6 7
	BlogRules7 @
.@ A
TitleMaxLengthA O
)O P
.P Q
WhenQ U
(U V
xV W
=>X Z
x[ \
.\ ]
Title] b
isc e
notf i
nullj n
)n o
;o p
RuleFor 
( 
x 
=> 
x 
. 
Body 
) 
. 
NotEmpty %
(% &
)& '
.' (
MaximumLength( 5
(5 6
	BlogRules6 ?
.? @
PostBodyMaxLength@ Q
)Q R
.R S
WhenS W
(W X
xX Y
=>Z \
x] ^
.^ _
Body_ c
isd f
notg j
nullk o
)o p
;p q
} 
}   
internal"" 
sealed""	 
class"" 
PatchPostEndpoint"" '
:""( )
MinimalEndpoint""* 9
<""9 :
PostsResource"": G
>""G H
{## 
public$$ 

override$$ 
RouteHandlerBuilder$$ '
Map$$( +
($$+ ,!
IEndpointRouteBuilder$$, A
builder$$B I
)$$I J
{%% 
return&& 
builder&& 
.'' 
MapPatch'' 
('' 
$str'' !
,''! "
Handle''# )
)'') *
.(( 
RequireAnyRole(( 
((( 
$str(( $
,(($ %
$str((& -
)((- .
.)) 
RequireScopes)) 
()) 
	AppScopes)) $
.))$ %
Write))% *
)))* +
;))+ ,
static++ 
async++ 
Task++ 
<++ 
Results++ !
<++! "
Ok++" $
<++$ %
PostResponse++% 1
>++1 2
,++2 3
ProblemHttpResult++4 E
>++E F
>++F G
Handle++H N
(++N O
Guid,, 
id,, 
,,, 
PatchPostBody-- 
body-- 
,-- 
ClaimsPrincipal.. 
user..  
,..  !

UpdatePost// 
feature// 
,// 
CancellationToken00 
cancellationToken00 /
)00/ 0
{11 	
var22 
result22 
=22 
await22 
feature22 &
.22& '
HandleAsync22' 2
(222 3
new223 6
UpdatePostRequest227 H
(22H I
id22I K
,22K L
body22M Q
.22Q R
Title22R W
,22W X
body22Y ]
.22] ^
Body22^ b
,22b c
user22d h
.22h i
ToCaller22i q
(22q r
)22r s
)22s t
,22t u
cancellationToken	22v á
)
22á à
;
22à â
return44 
result44 
.44 
ToHttpResult44 &
(44& '
)44' (
;44( )
}55 	
}66 
}77 ‡
SD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\GetPostsEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
class 
GetPostsEndpoint &
:' (
MinimalEndpoint) 8
<8 9
PostsResource9 F
>F G
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
string 
. 
Empty  
,  !
Handle" (
)( )
. 
RequireScopes 
( 
	AppScopes $
.$ %
Read% )
)) *
. 
ProducesPage 
< 
PostSummaryResponse -
>- .
(. /
)/ 0
;0 1
static 
async 
Task 
< 
Results !
<! "
Ok" $
<$ %
Page% )
<) *
PostSummaryResponse* =
>= >
>> ?
,? @
ProblemHttpResultA R
>R S
>S T
HandleU [
([ \
[ 
AsParameters 
] 
SearchPostsFilter ,
filter- 3
,3 4
ClaimsPrincipal 
user  
,  !
SearchPosts   
feature   
,    
CancellationToken!! 
cancellationToken!! /
)!!/ 0
{"" 	
var## 
result## 
=## 
await## 
feature## &
.##& '
HandleAsync##' 2
(##2 3
new##3 6
SearchPostsRequest##7 I
(##I J
filter##J P
,##P Q
user##R V
.##V W
ToCaller##W _
(##_ `
)##` a
)##a b
,##b c
cancellationToken##d u
)##u v
;##v w
return%% 
result%% 
.%% 
ToHttpResult%% &
(%%& '
)%%' (
;%%( )
}&& 	
}'' 
}(( Æ
RD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\GetPostEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
class 
GetPostEndpoint %
:& '
MinimalEndpoint( 7
<7 8
PostsResource8 E
>E F
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
Handle $
)$ %
. 
RequireScopes 
( 
	AppScopes $
.$ %
Read% )
)) *
;* +
static 
async 
Task 
< 
Results !
<! "
Ok" $
<$ %
PostResponse% 1
>1 2
,2 3
ProblemHttpResult4 E
>E F
>F G
HandleH N
(N O
string 
slug 
, 
ClaimsPrincipal 
user  
,  !
GetPost 
feature 
, 
CancellationToken 
cancellationToken /
)/ 0
{ 	
var 
result 
= 
await 
feature &
.& '
HandleAsync' 2
(2 3
new3 6
GetPostRequest7 E
(E F
slugF J
,J K
userL P
.P Q
ToCallerQ Y
(Y Z
)Z [
)[ \
,\ ]
cancellationToken^ o
)o p
;p q
return!! 
result!! 
.!! 
ToHttpResult!! &
(!!& '
)!!' (
;!!( )
}"" 	
}## 
}$$ æ
UD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\DeletePostEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
class 
DeletePostEndpoint (
:) *
MinimalEndpoint+ :
<: ;
PostsResource; H
>H I
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
	MapDelete 
( 
$str "
," #
Handle$ *
)* +
. 
RequireRoles 
( 
$str !
)! "
. 
RequireScopes 
( 
	AppScopes $
.$ %
Write% *
)* +
;+ ,
static 
async 
Task 
< 
Results !
<! "
	NoContent" +
,+ ,
ProblemHttpResult- >
>> ?
>? @
HandleA G
(G H
Guid 
id 
, 

DeletePost 
feature 
, 
CancellationToken 
cancellationToken /
)/ 0
{ 	
var 
result 
= 
await 
feature &
.& '
HandleAsync' 2
(2 3
new3 6
DeletePostRequest7 H
(H I
idI K
)K L
,L M
cancellationTokenN _
)_ `
;` a
return 
result 
. 
ToHttpResult &
(& '
)' (
;( )
}   	
}!! 
}"" ÿ 
UD:\git\aur.archetypes.api\src\App.Api\Resources\Posts\Endpoints\CreatePostEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Posts2 7
.7 8
	Endpoints8 A
;A B
internal 
sealed	 
record 
CreatePostBody %
(% &
string& ,
Title- 2
,2 3
string4 :
Body; ?
)? @
;@ A
internal 
sealed	 
class #
CreatePostBodyValidator -
:. /
RequestValidator0 @
<@ A
CreatePostBodyA O
>O P
{ 
public 
#
CreatePostBodyValidator "
(" #
)# $
{ 
RuleFor 
( 
x 
=> 
x 
. 
Title 
) 
. 
NotEmpty &
(& '
)' (
.( )
MaximumLength) 6
(6 7
	BlogRules7 @
.@ A
TitleMaxLengthA O
)O P
;P Q
RuleFor 
( 
x 
=> 
x 
. 
Body 
) 
. 
NotEmpty %
(% &
)& '
.' (
MaximumLength( 5
(5 6
	BlogRules6 ?
.? @
PostBodyMaxLength@ Q
)Q R
;R S
} 
}   
internal"" 
sealed""	 
class"" 
CreatePostEndpoint"" (
:"") *
MinimalEndpoint""+ :
<"": ;
PostsResource""; H
>""H I
{## 
public$$ 

override$$ 
RouteHandlerBuilder$$ '
Map$$( +
($$+ ,!
IEndpointRouteBuilder$$, A
builder$$B I
)$$I J
{%% 
return&& 
builder&& 
.'' 
MapPost'' 
('' 
string'' 
.'' 
Empty'' !
,''! "
Handle''# )
)'') *
.(( 
RequireAnyRole(( 
((( 
$str(( $
,(($ %
$str((& -
)((- .
.)) 
RequireScopes)) 
()) 
	AppScopes)) $
.))$ %
Write))% *
)))* +
.** 
WithIdempotency** 
(** 
)** 
;** 
static,, 
async,, 
Task,, 
<,, 
Results,, !
<,,! "
Created,," )
<,,) *
PostResponse,,* 6
>,,6 7
,,,7 8
ProblemHttpResult,,9 J
>,,J K
>,,K L
Handle,,M S
(,,S T
CreatePostBody-- 
body-- 
,--  
ClaimsPrincipal.. 
user..  
,..  !

CreatePost// 
feature// 
,// 
CancellationToken00 
cancellationToken00 /
)00/ 0
{11 	
var22 
result22 
=22 
await22 
feature22 &
.22& '
HandleAsync22' 2
(222 3
new223 6
CreatePostRequest227 H
(22H I
user22I M
.22M N
ToCaller22N V
(22V W
)22W X
.22X Y
Subject22Y `
,22` a
body22b f
.22f g
Title22g l
,22l m
body22n r
.22r s
Body22s w
)22w x
,22x y
cancellationToken	22z ã
)
22ã å
;
22å ç
return44 
result44 
.44 
ToHttpResult44 &
(44& '
post44' +
=>44, .
TypedResults44/ ;
.44; <
Created44< C
(44C D
$"44D F
$str44F P
{44P Q
post44Q U
.44U V
Slug44V Z
}44Z [
"44[ \
,44\ ]
post44^ b
)44b c
)44c d
;44d e
}55 	
}66 
}77 ⁄
DD:\git\aur.archetypes.api\src\App.Api\Resources\Feed\FeedResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Feed2 6
;6 7
internal 
sealed	 
class 
FeedResource "
:# $
ResourceGroup% 2
;2 3™
[D:\git\aur.archetypes.api\src\App.Api\Resources\Comments\Endpoints\ReportCommentEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Comments2 :
.: ;
	Endpoints; D
;D E
internal 
sealed	 
record 
ReportCommentBody (
(( )
string) /
Reason0 6
)6 7
;7 8
internal 
sealed	 
class &
ReportCommentBodyValidator 0
:1 2
RequestValidator3 C
<C D
ReportCommentBodyD U
>U V
{ 
public 
&
ReportCommentBodyValidator %
(% &
)& '
{ 
RuleFor 
( 
x 
=> 
x 
. 
Reason 
) 
. 
NotEmpty '
(' (
)( )
.) *
MaximumLength* 7
(7 8
	BlogRules8 A
.A B!
ReportReasonMaxLengthB W
)W X
;X Y
} 
} 
internal   
sealed  	 
class   !
ReportCommentEndpoint   +
:  , -
MinimalEndpoint  . =
<  = >
CommentsResource  > N
>  N O
{!! 
public"" 

override"" 
RouteHandlerBuilder"" '
Map""( +
(""+ ,!
IEndpointRouteBuilder"", A
builder""B I
)""I J
{## 
return$$ 
builder$$ 
.%% 
MapPost%% 
(%% 
$str%% .
,%%. /
Handle%%0 6
)%%6 7
.&& 
RequireScopes&& 
(&& 
	AppScopes&& $
.&&$ %
Write&&% *
)&&* +
.'' 
WithIdempotency'' 
('' 
)'' 
;'' 
static)) 
async)) 
Task)) 
<)) 
Results)) !
<))! "
Accepted))" *
<))* +!
CommentReportResponse))+ @
>))@ A
,))A B
ProblemHttpResult))C T
>))T U
>))U V
Handle))W ]
())] ^
Guid** 
postId** 
,** 
Guid++ 
	commentId++ 
,++ 
ReportCommentBody,, 
body,, "
,,," #
ClaimsPrincipal-- 
user--  
,--  !
ReportComment.. 
feature.. !
,..! "
CancellationToken// 
cancellationToken// /
)/// 0
{00 	
var11 
result11 
=11 
await11 
feature11 &
.11& '
HandleAsync11' 2
(112 3
new113 6 
ReportCommentRequest117 K
(11K L
user11L P
.11P Q
ToCaller11Q Y
(11Y Z
)11Z [
.11[ \
Subject11\ c
,11c d
postId11e k
,11k l
	commentId11m v
,11v w
body11x |
.11| }
Reason	11} É
)
11É Ñ
,
11Ñ Ö
cancellationToken
11Ü ó
)
11ó ò
;
11ò ô
return33 
result33 
.33 
ToHttpResult33 &
(33& '
report33' -
=>33. 0
TypedResults331 =
.33= >
Accepted33> F
(33F G
(33G H
string33H N
?33N O
)33O P
null33P T
,33T U
report33V \
)33\ ]
)33] ^
;33^ _
}44 	
}55 
}66 ˘
QD:\git\aur.archetypes.api\src\App.Api\Resources\Feed\Endpoints\GetFeedEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Feed2 6
.6 7
	Endpoints7 @
;@ A
internal 
sealed	 
class 
GetFeedEndpoint %
:& '
MinimalEndpoint( 7
<7 8
FeedResource8 D
>D E
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
string 
. 
Empty  
,  !
Handle" (
)( )
. 
RequireScopes 
( 
	AppScopes $
.$ %
Read% )
)) *
. 
ProducesCursorPage 
<  
PostSummaryResponse  3
>3 4
(4 5
)5 6
;6 7
static 
async 
Task 
< 
Results !
<! "
Ok" $
<$ %

CursorPage% /
</ 0
PostSummaryResponse0 C
>C D
>D E
,E F
ProblemHttpResultG X
>X Y
>Y Z
Handle[ a
(a b
[ 
AsParameters 
] 
StreamPostsRequest -
request. 5
,5 6
StreamPosts 
feature 
,  
CancellationToken 
cancellationToken /
)/ 0
{ 	
var   
result   
=   
await   
feature   &
.  & '
HandleAsync  ' 2
(  2 3
request  3 :
,  : ;
cancellationToken  < M
)  M N
;  N O
return"" 
result"" 
."" 
ToHttpResult"" &
(""& '
)""' (
;""( )
}## 	
}$$ 
}%% ï
YD:\git\aur.archetypes.api\src\App.Api\Resources\Comments\Endpoints\GetCommentsEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Comments2 :
.: ;
	Endpoints; D
;D E
internal 
sealed	 
class 
GetCommentsEndpoint )
:* +
MinimalEndpoint, ;
<; <
CommentsResource< L
>L M
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
string 
. 
Empty  
,  !
Handle" (
)( )
. 
RequireScopes 
( 
	AppScopes $
.$ %
Read% )
)) *
. 
ProducesCursorPage 
<  "
CommentSummaryResponse  6
>6 7
(7 8
)8 9
;9 :
static 
async 
Task 
< 
Results !
<! "
Ok" $
<$ %

CursorPage% /
</ 0"
CommentSummaryResponse0 F
>F G
>G H
,H I
ProblemHttpResultJ [
>[ \
>\ ]
Handle^ d
(d e
[ 
AsParameters 
] 
ListCommentsRequest .
request/ 6
,6 7
ListComments 
feature  
,  !
CancellationToken 
cancellationToken /
)/ 0
{ 	
var 
result 
= 
await 
feature &
.& '
HandleAsync' 2
(2 3
request3 :
,: ;
cancellationToken< M
)M N
;N O
return!! 
result!! 
.!! 
ToHttpResult!! &
(!!& '
)!!' (
;!!( )
}"" 	
}## 
}$$ π
[D:\git\aur.archetypes.api\src\App.Api\Resources\Comments\Endpoints\DeleteCommentEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Comments2 :
.: ;
	Endpoints; D
;D E
internal 
sealed	 
class !
DeleteCommentEndpoint +
:, -
MinimalEndpoint. =
<= >
CommentsResource> N
>N O
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
	MapDelete 
( 
$str )
,) *
Handle+ 1
)1 2
. 
RequireRoles 
( 
$str !
)! "
. 
RequireScopes 
( 
	AppScopes $
.$ %
Write% *
)* +
;+ ,
static 
async 
Task 
< 
Results !
<! "
	NoContent" +
,+ ,
ProblemHttpResult- >
>> ?
>? @
HandleA G
(G H
Guid 
postId 
, 
Guid 
	commentId 
, 
DeleteComment 
feature !
,! "
CancellationToken 
cancellationToken /
)/ 0
{ 	
var 
result 
= 
await 
feature &
.& '
HandleAsync' 2
(2 3
new3 6 
DeleteCommentRequest7 K
(K L
postIdL R
,R S
	commentIdT ]
)] ^
,^ _
cancellationToken` q
)q r
;r s
return   
result   
.   
ToHttpResult   &
(  & '
)  ' (
;  ( )
}!! 	
}"" 
}## ˇ
[D:\git\aur.archetypes.api\src\App.Api\Resources\Comments\Endpoints\CreateCommentEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Comments2 :
.: ;
	Endpoints; D
;D E
internal 
sealed	 
record 
CreateCommentBody (
(( )
string) /
Body0 4
)4 5
;5 6
internal 
sealed	 
class &
CreateCommentBodyValidator 0
:1 2
RequestValidator3 C
<C D
CreateCommentBodyD U
>U V
{ 
public 
&
CreateCommentBodyValidator %
(% &
)& '
{ 
RuleFor 
( 
x 
=> 
x 
. 
Body 
) 
. 
NotEmpty %
(% &
)& '
.' (
MaximumLength( 5
(5 6
	BlogRules6 ?
.? @ 
CommentBodyMaxLength@ T
)T U
;U V
} 
} 
internal   
sealed  	 
class   !
CreateCommentEndpoint   +
:  , -
MinimalEndpoint  . =
<  = >
CommentsResource  > N
>  N O
{!! 
public"" 

override"" 
RouteHandlerBuilder"" '
Map""( +
(""+ ,!
IEndpointRouteBuilder"", A
builder""B I
)""I J
{## 
return$$ 
builder$$ 
.%% 
MapPost%% 
(%% 
string%% 
.%% 
Empty%% !
,%%! "
Handle%%# )
)%%) *
.&& 
RequireScopes&& 
(&& 
	AppScopes&& $
.&&$ %
Write&&% *
)&&* +
.'' 
WithIdempotency'' 
('' 
)'' 
;'' 
static)) 
async)) 
Task)) 
<)) 
Results)) !
<))! "
Created))" )
<))) *
CommentResponse))* 9
>))9 :
,)): ;
ProblemHttpResult))< M
>))M N
>))N O
Handle))P V
())V W
Guid** 
postId** 
,** 
CreateCommentBody++ 
body++ "
,++" #
ClaimsPrincipal,, 
user,,  
,,,  !
CreateComment-- 
feature-- !
,--! "
CancellationToken.. 
cancellationToken.. /
)../ 0
{// 	
var00 
result00 
=00 
await00 
feature00 &
.00& '
HandleAsync00' 2
(002 3
new003 6 
CreateCommentRequest007 K
(00K L
user00L P
.00P Q
ToCaller00Q Y
(00Y Z
)00Z [
.00[ \
Subject00\ c
,00c d
postId00e k
,00k l
body00m q
.00q r
Body00r v
)00v w
,00w x
cancellationToken	00y ä
)
00ä ã
;
00ã å
return22 
result22 
.22 
ToHttpResult22 &
(22& '
comment22' .
=>22/ 1
TypedResults222 >
.22> ?
Created22? F
(22F G
$"22G I
$str22I S
{22S T
postId22T Z
}22Z [
$str22[ d
"22d e
,22e f
comment22g n
)22n o
)22o p
;22p q
}33 	
}44 
}55 ˆ
LD:\git\aur.archetypes.api\src\App.Api\Resources\Comments\CommentsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Comments2 :
;: ;
internal 
sealed	 
class 
CommentsResource &
:' (
ResourceGroup) 6
{ 
public 

override 
string 
Name 
=>  "
$str# A
;A B
} ô
XD:\git\aur.archetypes.api\src\App.Api\Resources\Authors\Endpoints\PutMyAuthorEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Authors2 9
.9 :
	Endpoints: C
;C D
internal 
sealed	 
record 
PutAuthorBody $
($ %
string% +
Handle, 2
,2 3
string4 :
DisplayName; F
)F G
;G H
internal 
sealed	 
class "
PutAuthorBodyValidator ,
:- .
RequestValidator/ ?
<? @
PutAuthorBody@ M
>M N
{ 
public 
"
PutAuthorBodyValidator !
(! "
)" #
{ 
RuleFor 
( 
x 
=> 
x 
. 
Handle 
) 
. 
NotEmpty '
(' (
)( )
.) *
Length* 0
(0 1
	BlogRules1 :
.: ;
HandleMinLength; J
,J K
	BlogRulesL U
.U V
HandleMaxLengthV e
)e f
.f g
Matchesg n
(n o
AppPatternso z
.z {
Handle	{ Å
)
Å Ç
. 
WithMessage 
( 
$str 
)	 Ä
;
Ä Å
RuleFor 
( 
x 
=> 
x 
. 
DisplayName "
)" #
.# $
NotEmpty$ ,
(, -
)- .
.. /
MaximumLength/ <
(< =
	BlogRules= F
.F G 
DisplayNameMaxLengthG [
)[ \
;\ ]
} 
} 
internal   
sealed  	 
class   
PutMyAuthorEndpoint   )
:  * +
MinimalEndpoint  , ;
<  ; <
AuthorsResource  < K
>  K L
{!! 
public"" 

override"" 
RouteHandlerBuilder"" '
Map""( +
(""+ ,!
IEndpointRouteBuilder"", A
builder""B I
)""I J
{## 
return$$ 
builder$$ 
.%% 
MapPut%% 
(%% 
$str%% 
,%% 
Handle%%  
)%%  !
.&& 
RequireScopes&& 
(&& 
	AppScopes&& $
.&&$ %
Write&&% *
)&&* +
;&&+ ,
static(( 
async(( 
Task(( 
<(( 
Results(( !
<((! "
Ok((" $
<(($ %
AuthorResponse((% 3
>((3 4
,((4 5
ProblemHttpResult((6 G
>((G H
>((H I
Handle((J P
(((P Q
PutAuthorBody)) 
body)) 
,)) 
ClaimsPrincipal** 
user**  
,**  !
UpsertAuthor++ 
feature++  
,++  !
CancellationToken,, 
cancellationToken,, /
),,/ 0
{-- 	
var.. 
result.. 
=.. 
await.. 
feature.. &
...& '
HandleAsync..' 2
(..2 3
new..3 6
UpsertAuthorRequest..7 J
(..J K
user..K O
...O P
ToCaller..P X
(..X Y
)..Y Z
...Z [
Subject..[ b
,..b c
body..d h
...h i
Handle..i o
,..o p
body..q u
...u v
DisplayName	..v Å
)
..Å Ç
,
..Ç É
cancellationToken
..Ñ ï
)
..ï ñ
;
..ñ ó
return00 
result00 
.00 
ToHttpResult00 &
(00& '
)00' (
;00( )
}11 	
}22 
}33 Ê
JD:\git\aur.archetypes.api\src\App.Api\Resources\Authors\AuthorsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
	Resources( 1
.1 2
Authors2 9
;9 :
internal 
sealed	 
class 
AuthorsResource %
:& '
ResourceGroup( 5
;5 6°+
0D:\git\aur.archetypes.api\src\App.Api\Program.cs
WebApplication 
app 
; 
try 
{ 
var 
builder 
= 
WebApplication  
.  !
CreateBuilder! .
(. /
args/ 3
)3 4
;4 5
builder 
. 	(
ConfigureStandardSerializers	 %
(% &
)& '
. 	
LoadAppSettings	 
( 
out 
var  
appSettings! ,
), -
. 	$
ConfigureStandardHosting	 !
(! "
appSettings" -
.- .
Hosting. 5
)5 6
. 	 
AddStandardTelemetry	 
( 
appSettings )
.) *
AppInformation* 8
,8 9
appSettings: E
.E F
	TelemetryF O
)O P
. 	'
AddStandardMinimalEndpoints	 $
<$ %
Program% ,
>, -
(- .
out. 1
var2 5
	endpoints6 ?
)? @
. 	
AddAppScalar	 
( 
appSettings !
,! "
	endpoints# ,
), -
. 	&
AddStandardRequestPipeline	 #
(# $
appSettings$ /
./ 0
RequestPipeline0 ?
)? @
;@ A
AppDiagnostics!! 
.!! 

Initialize!! 
(!! 
appSettings!! )
.!!) *
AppInformation!!* 8
.!!8 9
ActivitySource!!9 G
)!!G H
;!!H I
builder## 
.## 
Services## 
.$$ 	
AddSingleton$$	 
($$ 
TimeProvider$$ "
.$$" #
System$$# )
)$$) *
.%% 	
AddSingleton%%	 
(%% 
appSettings%% !
.%%! "

Pagination%%" ,
)%%, -
.&& 	$
AddStandardApiVersioning&&	 !
(&&! "
	endpoints&&" +
)&&+ ,
.'' 	(
AddStandardAuthConfiguration''	 %
(''% &
appSettings''& 1
.''1 2
IdentityProvider''2 B
,''B C
appSettings''D O
.''O P
ApiKeys''P W
)''W X
.(( 	#
AddStandardHealthChecks((	  
(((  !
appSettings((! ,
.((, -
AppInformation((- ;
,((; <
appSettings((= H
.((H I
HealthChecks((I U
,((U V
appSettings((W b
.((b c
HttpDependencies((c s
)((s t
.)) 	
AddAppValidators))	 
()) 
appSettings)) %
)))% &
.** 	
AddAppFeatures**	 
(** 
)** 
.++ 	
AddAppInfra++	 
(++ 
appSettings++  
)++  !
;++! "
app-- 
=-- 	
builder--
 
.-- 
Build-- 
(-- 
)-- 
;-- 
app// 
.00 	&
UseStandardRequestPipeline00	 #
(00# $
appSettings00$ /
.00/ 0
RequestPipeline000 ?
)00? @
.11 	
UseAuthentication11	 
(11 
)11 
.22 	
UseAuthorization22	 
(22 
)22 
.33 	'
UseStandardEndpointPolicies33	 $
(33$ %
)33% &
;33& '
app55 
.55 
MapAppScalar55 
(55 
appSettings55  
)55  !
;55! "
app77 
.77 #
MapStandardHealthChecks77 
(77  
appSettings77  +
.77+ ,
AppInformation77, :
)77: ;
;77; <
app99 
.99 
MapAppResources99 
(99 
	endpoints99 !
)99! "
;99" #
}:: 
catch;; 
(;; 
	Exception;; 
ex;; 
);; 
when;; 
(;; 
ex;; 
is;;  
not;;! $ 
HostAbortedException;;% 9
);;9 :
{<< 
await== 	
Console==
 
.== 
Error== 
.== 
WriteLineAsync== &
(==& '
$"==' )
$str==) F
{==F G
ex==G I
}==I J
"==J K
)==K L
;==L M
return?? 

	ExitCodes?? 
.?? 
ConfigurationError?? '
;??' (
}@@ 
tryBB 
{CC 
appDD 
.DD 
LoggerDD 
.DD 
LogInformationDD 
(DD 
$strDD =
)DD= >
;DD> ?
awaitFF 	
appFF
 
.FF 
RunAsyncFF 
(FF 
)FF 
;FF 
returnHH 

	ExitCodesHH 
.HH 
SuccessHH 
;HH 
}II 
catchJJ 
(JJ 
	ExceptionJJ 
exJJ 
)JJ 
whenJJ 
(JJ 
exJJ 
isJJ  
notJJ! $ 
HostAbortedExceptionJJ% 9
)JJ9 :
{KK 
awaitLL 	
ConsoleLL
 
.LL 
ErrorLL 
.LL 
WriteLineAsyncLL &
(LL& '
$"LL' )
$strLL) K
{LLK L
exLLL N
}LLN O
"LLO P
)LLP Q
;LLQ R
returnNN 

	ExitCodesNN 
.NN 
RuntimeErrorNN !
;NN! "
}OO ˚
MD:\git\aur.archetypes.api\src\App.Api\Playground\Timeouts\TimeoutsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Timeouts3 ;
;; <
internal 
sealed	 
class 
TimeoutsResource &
(& '
)' (
:) *
PlaygroundResource+ =
(= >
$str> H
,H I
$strJ \
)\ ]
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} Ò
YD:\git\aur.archetypes.api\src\App.Api\Playground\Timeouts\Endpoints\GetTimeoutEndpoint.cs
	namespace

 	
Aurore


 
.

 

Archetypes

 
.

 
Api

 
.

  
App

  #
.

# $
Api

$ '
.

' (

Playground

( 2
.

2 3
Timeouts

3 ;
.

; <
	Endpoints

< E
;

E F
internal 
sealed	 
class 
GetTimeoutEndpoint (
:) *
MinimalEndpoint+ :
<: ;
TimeoutsResource; K
>K L
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str #
,# $
HandleAsync% 0
)0 1
. 
WithMetadata 
( 
new #
RequestTimeoutAttribute 5
(5 6
milliseconds6 B
:B C
$numD H
)H I
)I J
;J K
static 
async 
Task 
< 
IResult !
>! "
HandleAsync# .
(. /
int/ 2
seconds3 :
,: ;
CancellationToken< M
cancellationTokenN _
)_ `
{ 	
await 
Task 
. 
Delay 
( 
TimeSpan %
.% &
FromSeconds& 1
(1 2
seconds2 9
)9 :
,: ;
cancellationToken< M
)M N
;N O
return 
TypedResults 
.  
Ok  "
(" #
new# &
{' (
WaitedSeconds) 6
=7 8
seconds9 @
}A B
)B C
;C D
} 	
} 
} “
MD:\git\aur.archetypes.api\src\App.Api\Playground\Shared\PlaygroundResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Shared3 9
;9 :
internal 
abstract	 
class 
PlaygroundResource *
(* +
string+ 1
topic2 7
,7 8
string9 ?
title@ E
)E F
:G H
ResourceGroupI V
{ 
public 

override 
string 
Name 
=>  "
$"# %
$str% 0
{0 1
topic1 6
}6 7
"7 8
;8 9
public		 

override		 
string		 
[		 
]		 
Tags		 !
=>		" $
[		% &
$"		& (
$str		( 4
{		4 5
title		5 :
}		: ;
"		; <
]		< =
;		= >
}

 ˇ+
SD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Shared\PlaygroundOrders.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
Shared; A
;A B
internal

 
static

	 
class

 
PlaygroundOrders

 &
{ 
private 
static 
readonly  
ConcurrentDictionary 0
<0 1
int1 4
,4 5
PlaygroundOrder6 E
>E F
_ordersG N
=O P
newQ T
(T U
)U V
{ 
[ 	
$num	 

]
 
= 
new 
PlaygroundOrder !
(! "
$num" #
,# $
$str% 6
,6 7
$num8 9
,9 :
$str; D
)D E
,E F
[ 	
$num	 

]
 
= 
new 
PlaygroundOrder !
(! "
$num" #
,# $
$str% 8
,8 9
$num: ;
,; <
$str= F
)F G
,G H
} 
; 
private 
static 
int 
_lastId 
=  
$num! "
;" #
public 

static 
Result 
< 
PlaygroundOrder (
>( )
Get* -
(- .
int. 1
id2 4
)4 5
{ 
return 
_orders 
. 
TryGetValue "
(" #
id# %
,% &
out' *
var+ .
order/ 4
)4 5
?6 7
order8 =
:> ?
PlaygroundErrors@ P
.P Q
OrdersQ W
.W X
NotFoundX `
;` a
} 
public 

static 
Result 
< 
PlaygroundOrder (
>( )
Create* 0
(0 1(
CreatePlaygroundOrderRequest1 M
requestN U
)U V
{ 
if 

( 
_orders 
. 
Values 
. 
Any 
( 
x  
=>! #
x$ %
.% &
Email& +
==, .
request/ 6
.6 7
Email7 <
)< =
)= >
return 
PlaygroundErrors #
.# $
Orders$ *
.* +
DuplicateEmail+ 9
;9 :
var 
order 
= 
new 
PlaygroundOrder '
(' (
Interlocked( 3
.3 4
	Increment4 =
(= >
ref> A
_lastIdB I
)I J
,J K
requestL S
.S T
EmailT Y
,Y Z
request[ b
.b c
Quantityc k
,k l
$strm v
)v w
;w x
_orders 
[ 
order 
. 
Id 
] 
= 
order !
;! "
return!! 
order!! 
;!! 
}"" 
public$$ 

static$$ 
Result$$ 
<$$ 
PlaygroundOrder$$ (
>$$( )
Cancel$$* 0
($$0 1
int$$1 4
id$$5 7
)$$7 8
{%% 
var&& 
found&& 
=&& 
Get&& 
(&& 
id&& 
)&& 
;&& 
if(( 

((( 
found(( 
.(( 
	IsFailure(( 
)(( 
return)) 
found)) 
.)) 
Error)) 
!)) 
;))  
var++ 

validation++ 
=++ 
new++ *
CancelPlaygroundOrderValidator++ ;
(++; <
)++< =
.++= >
Validate++> F
(++F G
found++G L
.++L M
Value++M R
)++R S
;++S T
if-- 

(-- 

validation-- 
.-- 
IsValid-- 
is-- !
false--" '
)--' (
return.. 

validation.. 
... 
ToApplicationError.. 0
(..0 1
PlaygroundErrors..1 A
...A B
Orders..B H
...H I
CannotCancel..I U
)..U V
;..V W
var00 
	cancelled00 
=00 
found00 
.00 
Value00 #
with00$ (
{00) *
Status00+ 1
=002 3
$str004 ?
}00@ A
;00A B
_orders11 
[11 
id11 
]11 
=11 
	cancelled11 
;11  
return33 
	cancelled33 
;33 
}44 
public66 

static66 
Result66 
Delete66 
(66  
int66  #
id66$ &
)66& '
{77 
return88 
_orders88 
.88 
	TryRemove88  
(88  !
id88! #
,88# $
out88% (
_88) *
)88* +
?88, -
Result88. 4
.884 5
Success885 <
:88= >
PlaygroundErrors88? O
.88O P
Orders88P V
.88V W
NotFound88W _
;88_ `
}99 
}:: À
RD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Shared\PlaygroundOrder.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
Shared; A
;A B
internal 
sealed	 
record 
PlaygroundOrder &
(& '
int' *
Id+ -
,- .
string/ 5
Email6 ;
,; <
int= @
QuantityA I
,I J
stringK Q
StatusR X
)X Y
;Y ZÚ
SD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Shared\PlaygroundErrors.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
Shared; A
;A B
internal 
sealed	 
class 
PlaygroundErrors &
:' (
StandardErrors) 7
{ 
private 
PlaygroundErrors 
( 
) 
{ 
}		 
public 

static 
class 
Orders 
{ 
public 
static 
readonly 
ApplicationError /
NotFound0 8
=9 :
ApplicationError 
. 
NotFound %
(% &
$str& 7
,7 8
$str9 S
)S T
;T U
public 
static 
readonly 
ApplicationError /
DuplicateEmail0 >
=? @
ApplicationError 
. 
Conflict %
(% &
$str& =
,= >
$str? h
)h i
;i j
public 
static 
readonly 
ApplicationError /
CannotCancel0 <
== >
ApplicationError 
. 
Conflict %
(% &
$str& ;
,; <
$str= d
)d e
;e f
} 
} ˛
_D:\git\aur.archetypes.api\src\App.Api\Playground\Results\Shared\CreatePlaygroundOrderRequest.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
Shared; A
;A B
internal 
sealed	 
record (
CreatePlaygroundOrderRequest 3
(3 4
string4 :
Email; @
,@ A
intB E
QuantityF N
)N O
;O P˜
KD:\git\aur.archetypes.api\src\App.Api\Playground\Results\ResultsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
;: ;
internal 
sealed	 
class 
ResultsResource %
(% &
)& '
:( )
PlaygroundResource* <
(< =
$str= F
,F G
$strH \
)\ ]
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} ˚
`D:\git\aur.archetypes.api\src\App.Api\Playground\Results\Endpoints\GetPlaygroundOrderEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
	Endpoints; D
;D E
internal

 
sealed

	 
class

 &
GetPlaygroundOrderEndpoint

 0
:

1 2
MinimalEndpoint

3 B
<

B C
ResultsResource

C R
>

R S
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str /
,/ 0
Handle1 7
)7 8
;8 9
static 
Results 
< 
Ok 
< 
PlaygroundOrder )
>) *
,* +
ProblemHttpResult, =
>= >
Handle? E
(E F
intF I
idJ L
)L M
{ 	
return 
PlaygroundOrders #
.# $
Get$ '
(' (
id( *
)* +
.+ ,
ToHttpResult, 8
(8 9
)9 :
;: ;
} 	
} 
} ”
cD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Endpoints\DeletePlaygroundOrderEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
	Endpoints; D
;D E
internal

 
sealed

	 
class

 )
DeletePlaygroundOrderEndpoint

 3
:

4 5
MinimalEndpoint

6 E
<

E F
ResultsResource

F U
>

U V
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
	MapDelete  
(  !
$str! 2
,2 3
Handle4 :
): ;
;; <
static 
Results 
< 
	NoContent  
,  !
ProblemHttpResult" 3
>3 4
Handle5 ;
(; <
int< ?
id@ B
)B C
{ 	
return 
PlaygroundOrders #
.# $
Delete$ *
(* +
id+ -
)- .
.. /
ToHttpResult/ ;
(; <
)< =
;= >
} 	
} 
} 
cD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Endpoints\CreatePlaygroundOrderEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Results3 :
.: ;
	Endpoints; D
;D E
internal 
sealed	 
class *
CreatePlaygroundOrderValidator 4
:5 6
RequestValidator7 G
<G H(
CreatePlaygroundOrderRequestH d
>d e
{ 
public 
*
CreatePlaygroundOrderValidator )
() *
)* +
{ 
RuleFor 
( 
x 
=> 
x 
. 
Email 
) 
. 
NotEmpty &
(& '
)' (
.( )
EmailAddress) 5
(5 6
)6 7
;7 8
RuleFor 
( 
x 
=> 
x 
. 
Quantity 
)  
.  !
InclusiveBetween! 1
(1 2
$num2 3
,3 4
$num5 8
)8 9
;9 :
} 
} 
internal 
sealed	 
class )
CreatePlaygroundOrderEndpoint 3
:4 5
MinimalEndpoint6 E
<E F
ResultsResourceF U
>U V
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapPost 
( 
$str '
,' (
Handle) /
)/ 0
;0 1
static 
Results 
< 
Created 
< 
PlaygroundOrder .
>. /
,/ 0
ProblemHttpResult1 B
>B C
HandleD J
(J K(
CreatePlaygroundOrderRequestK g
requesth o
)o p
{ 	
return 
PlaygroundOrders #
. 
Create 
( 
request 
)  
.   
ToHttpResult   
(   
order   #
=>  $ &
TypedResults  ' 3
.  3 4
Created  4 ;
(  ; <
$"  < >
$str  > \
{  \ ]
order  ] b
.  b c
Id  c e
}  e f
"  f g
,  g h
order  i n
)  n o
)  o p
;  p q
}!! 	
}"" 
}## ß
cD:\git\aur.archetypes.api\src\App.Api\Playground\Results\Endpoints\CancelPlaygroundOrderEndpoint.cs
	namespace

 	
Aurore


 
.

 

Archetypes

 
.

 
Api

 
.

  
App

  #
.

# $
Api

$ '
.

' (

Playground

( 2
.

2 3
Results

3 :
.

: ;
	Endpoints

; D
;

D E
internal 
sealed	 
class *
CancelPlaygroundOrderValidator 4
:5 6
FeatureValidator7 G
<G H
PlaygroundOrderH W
>W X
{ 
public 
*
CancelPlaygroundOrderValidator )
() *
)* +
{ 
RuleFor 
( 
x 
=> 
x 
. 
Status 
) 
. 
Equal $
($ %
$str% .
). /
./ 0
WithMessage0 ;
(; <
$str< c
)c d
;d e
} 
} 
internal 
sealed	 
class )
CancelPlaygroundOrderEndpoint 3
:4 5
MinimalEndpoint6 E
<E F
ResultsResourceF U
>U V
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapPost 
( 
$str 7
,7 8
Handle9 ?
)? @
;@ A
static 
Results 
< 
Ok 
< 
PlaygroundOrder )
>) *
,* +
ProblemHttpResult, =
>= >
Handle? E
(E F
intF I
idJ L
)L M
{ 	
return 
PlaygroundOrders #
.# $
Cancel$ *
(* +
id+ -
)- .
.. /
ToHttpResult/ ;
(; <
)< =
;= >
} 	
} 
} ã
UD:\git\aur.archetypes.api\src\App.Api\Playground\RateLimiting\RateLimitingResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
RateLimiting3 ?
;? @
internal 
sealed	 
class  
RateLimitingResource *
(* +
)+ ,
:- .
PlaygroundResource/ A
(A B
$strB Q
,Q R
$strS b
)b c
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} õ
]D:\git\aur.archetypes.api\src\App.Api\Playground\RateLimiting\Endpoints\GetLimitedEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
RateLimiting3 ?
.? @
	Endpoints@ I
;I J
internal		 
sealed			 
class		 
GetLimitedEndpoint		 (
:		) *
MinimalEndpoint		+ :
<		: ; 
RateLimitingResource		; O
>		O P
{

 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
(  
TimeProvider  ,
time- 1
)1 2
=>3 5
TypedResults6 B
.B C
OkC E
(E F
newF I
{J K
	TimestampL U
=V W
timeX \
.\ ]
	GetUtcNow] f
(f g
)g h
}i j
)j k
)k l
. 
RequireRateLimiting  
(  !
$str! (
)( )
;) *
} 
} 
cD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Shared\StreamPlaygroundPeopleRequest.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
Shared> D
;D E
internal 
sealed	 
class )
StreamPlaygroundPeopleRequest 3
:4 5"
TimestampCursorRequest6 L
{ 
public 

string 
? 
Name 
{ 
get 
; 
init #
;# $
}% &
} Ó
cD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Shared\SearchPlaygroundPeopleRequest.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
Shared> D
;D E
internal 
sealed	 
class )
SearchPlaygroundPeopleRequest 3
:4 5 
TimestampPageRequest6 J
{ 
public 

string 
? 
Name 
{ 
get 
; 
init #
;# $
}% &
} È
VD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Shared\PlaygroundPerson.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
Shared> D
;D E
internal 
sealed	 
record 
PlaygroundPerson '
(' (
int( +
Id, .
,. /
string0 6
Name7 ;
,; <
DateTimeOffset= K
	CreatedAtL U
,U V
DateTimeOffsetW e
	UpdatedAtf o
)o p
;p q®2
VD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Shared\PlaygroundPeople.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
Shared> D
;D E
internal		 
static			 
class		 
PlaygroundPeople		 &
{

 
private 
static 
readonly 
SortMap #
<# $
PlaygroundPerson$ 4
>4 5
_sort6 ;
=< =
SortMap> E
.E F
ForF I
<I J
PlaygroundPersonJ Z
>Z [
([ \
)\ ]
. 	
Allow	 
( 
x 
=> 
x 
. 
Name 
) 
. 	
Allow	 
( 
x 
=> 
x 
. 
	CreatedAt 
)  
. 	
ById	 
( 
) 
; 
private 
static 
readonly 
PlaygroundPerson ,
[, -
]- .
_people/ 6
=7 8
[ 
.. 


Enumerable 
. 
Range 
( 
$num 
, 
$num !
)! "
." #
Select# )
() *
i* +
=>, .
new/ 2
PlaygroundPerson3 C
(C D
i 
, 
$" 
$str 
{ 
( 
char 
) 
( 
$char  
+! "
(# $
i$ %
%& '
$num( *
)* +
)+ ,
}, -
{- .
i. /
:/ 0
$str0 2
}2 3
"3 4
,4 5
new 
DateTimeOffset 
( 
$num #
,# $
$num% &
,& '
$num( )
,) *
$num+ ,
,, -
$num. /
,/ 0
$num1 2
,2 3
TimeSpan4 <
.< =
Zero= A
)A B
.B C
AddDaysC J
(J K
iK L
)L M
,M N
new 
DateTimeOffset 
( 
$num #
,# $
$num% &
,& '
$num( )
,) *
$num+ ,
,, -
$num. /
,/ 0
$num1 2
,2 3
TimeSpan4 <
.< =
Zero= A
)A B
.B C
AddDaysC J
(J K
iK L
)L M
)M N
)N O
,O P
] 
; 
public 

static 
Result 
< 
Page 
< 
PlaygroundPerson .
>. /
>/ 0
Search1 7
(7 8)
SearchPlaygroundPeopleRequest %
request& -
,- .
PaginationOptions 
options !
)! "
{ 
var 
filtered 
= 
Filter 
( 
request %
.% &
Name& *
,* +
request, 3
)3 4
;4 5
return 
filtered 
. 
	IsFailure !
?" #
filtered$ ,
., -
Error- 2
!2 3
:4 5
filtered6 >
.> ?
Value? D
.D E
ToPageE K
(K L
requestL S
,S T
optionsU \
,\ ]
_sort^ c
)c d
;d e
}   
public"" 

static"" 
Result"" 
<"" 

CursorPage"" #
<""# $
PlaygroundPerson""$ 4
>""4 5
>""5 6
Stream""7 =
(""= >)
StreamPlaygroundPeopleRequest## %
request##& -
,##- .
PaginationOptions$$ 
options$$ !
)$$! "
{%% 
var&& 
filtered&& 
=&& 
Filter&& 
(&& 
request&& %
.&&% &
Name&&& *
,&&* +
request&&, 3
)&&3 4
;&&4 5
return(( 
filtered(( 
.(( 
	IsFailure(( !
?((" #
filtered(($ ,
.((, -
Error((- 2
!((2 3
:((4 5
filtered((6 >
.((> ?
Value((? D
.((D E
ToCursorPage((E Q
(((Q R
request((R Y
,((Y Z
options(([ b
,((b c
_sort((d i
)((i j
;((j k
})) 
private++ 
static++ 
Result++ 
<++ 

IQueryable++ $
<++$ %
PlaygroundPerson++% 5
>++5 6
>++6 7
Filter++8 >
(++> ?
string++? E
?++E F
name++G K
,++K L
ITimestampFilter++M ]

timestamps++^ h
)++h i
{,, 
var-- 
query-- 
=-- 
_people-- 
.-- 
AsQueryable-- '
(--' (
)--( )
;--) *
if// 

(// 
string// 
.// 
IsNullOrWhiteSpace// %
(//% &
name//& *
)//* +
is//, .
false/// 4
)//4 5
query00 
=00 
query00 
.00 
Where00 
(00  
x00  !
=>00" $
x00% &
.00& '
Name00' +
.00+ ,
Contains00, 4
(004 5
name005 9
,009 :
StringComparison00; K
.00K L
OrdinalIgnoreCase00L ]
)00] ^
)00^ _
;00_ `
return22 
query22 
.22 
WhereTimestamps22 $
(22$ %

timestamps22% /
,22/ 0
x221 2
=>223 5
x226 7
.227 8
	CreatedAt228 A
,22A B
x22C D
=>22E G
x22H I
.22I J
	UpdatedAt22J S
)22S T
;22T U
}33 
}44 É
QD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\PaginationResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
;= >
internal 
sealed	 
class 
PaginationResource (
(( )
)) *
:+ ,
PlaygroundResource- ?
(? @
$str@ L
,L M
$strN Z
)Z [
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} ò
jD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Endpoints\GetPlaygroundPeopleStreamEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
	Endpoints> G
;G H
internal 
sealed	 
class -
!GetPlaygroundPeopleStreamEndpoint 7
:8 9
MinimalEndpoint: I
<I J
PaginationResourceJ \
>\ ]
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str #
,# $
Handle% +
)+ ,
. 
ProducesCursorPage 
<  
PlaygroundPerson  0
>0 1
(1 2
)2 3
;3 4
static 
Results 
< 
Ok 
< 

CursorPage $
<$ %
PlaygroundPerson% 5
>5 6
>6 7
,7 8
ProblemHttpResult9 J
>J K
HandleL R
(R S
[ 
AsParameters 
] )
StreamPlaygroundPeopleRequest 8
request9 @
,@ A
PaginationOptions 
options %
)% &
{ 	
return 
PlaygroundPeople #
.# $
Stream$ *
(* +
request+ 2
,2 3
options4 ;
); <
.< =
ToHttpResult= I
(I J
)J K
;K L
} 	
} 
} Ä
dD:\git\aur.archetypes.api\src\App.Api\Playground\Pagination\Endpoints\GetPlaygroundPeopleEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3

Pagination3 =
.= >
	Endpoints> G
;G H
internal 
sealed	 
class '
GetPlaygroundPeopleEndpoint 1
:2 3
MinimalEndpoint4 C
<C D
PaginationResourceD V
>V W
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
Handle $
)$ %
. 
ProducesPage 
< 
PlaygroundPerson *
>* +
(+ ,
), -
;- .
static 
Results 
< 
Ok 
< 
Page 
< 
PlaygroundPerson /
>/ 0
>0 1
,1 2
ProblemHttpResult3 D
>D E
HandleF L
(L M
[ 
AsParameters 
] )
SearchPlaygroundPeopleRequest 8
request9 @
,@ A
PaginationOptions 
options %
)% &
{ 	
return 
PlaygroundPeople #
.# $
Search$ *
(* +
request+ 2
,2 3
options4 ;
); <
.< =
ToHttpResult= I
(I J
)J K
;K L
} 	
} 
} È
ID:\git\aur.archetypes.api\src\App.Api\Playground\Layers\LayersResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Layers3 9
;9 :
internal 
sealed	 
class 
LayersResource $
($ %
)% &
:' (
PlaygroundResource) ;
(; <
$str< D
,D E
$strF Y
)Y Z
{		 
internal

 
const

 
string

 
ConfiguredByHeader

 ,
=

- .
$str

/ @
;

@ A
public 

override "
MinimalEndpointVersion *
[* +
]+ ,
Versions- 5
=>6 8
[9 :
$num: ;
,; <
new= @
(@ A
$numA B
,B C
trueD H
)H I
]I J
;J K
public 

override 
int 
[ 
] 
Problems "
=># %
[& '
StatusCodes' 2
.2 3
Status404NotFound3 D
,D E
StatusCodesF Q
.Q R
Status409ConflictR c
,c d
StatusCodese p
.p q)
Status500InternalServerError	q ç
]
ç é
;
é è
public 

override 
void 
	Configure "
(" #
RouteGroupBuilder# 4
builder5 <
)< =
{ 
builder 
. 
ProducesProblem 
( 
StatusCodes (
.( )
Status404NotFound) :
): ;
. 
AddEndpointFilter 
( 
(  
context  '
,' (
next) -
)- .
=>/ 1
{ 
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
ConfiguredByHeader5 G
]G H
=I J
$strK R
;R S
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
$str5 D
]D E
=F G
$strH N
;N O
return 
next 
( 
context #
)# $
;$ %
} 
) 
; 
} 
} Ü
YD:\git\aur.archetypes.api\src\App.Api\Playground\Layers\Endpoints\GetHeadersV2Endpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Layers3 9
.9 :
	Endpoints: C
;C D
internal 
sealed	 
class  
GetHeadersV2Endpoint *
:+ ,
MinimalEndpoint- <
<< =
LayersResource= K
>K L
{		 
public

 

override

 
int

 
Version

 
=>

  "
$num

# $
;

$ %
public 

override 
string 
Name 
=>  "
$str# 1
;1 2
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
(  
)  !
=>" $
TypedResults% 1
.1 2
Ok2 4
(4 5
new5 8
{9 :
Message; B
=C D
$strE c
}d e
)e f
)f g
. 
AllowAnonymous 
( 
) 
; 
} 
} ˚
WD:\git\aur.archetypes.api\src\App.Api\Playground\Layers\Endpoints\GetHeadersEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Layers3 9
.9 :
	Endpoints: C
;C D
internal 
sealed	 
class 
GetHeadersEndpoint (
:) *
MinimalEndpoint+ :
<: ;
LayersResource; I
>I J
{		 
public

 

override

 
string

 
Summary

 "
=>

# %
$str

& [
;

[ \
public 

override 
string 
Description &
=>' )
$str* g
;g h
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
(  
)  !
=>" $
TypedResults% 1
.1 2
Ok2 4
(4 5
new5 8
{9 :
Message; B
=C D
$str	E Ä
}
Å Ç
)
Ç É
)
É Ñ
. 
AllowAnonymous 
( 
) 
. 
ProducesProblem 
( 
StatusCodes (
.( )
Status404NotFound) :
): ;
. 
ProducesProblem 
( 
StatusCodes (
.( )(
Status422UnprocessableEntity) E
)E F
. 
AddEndpointFilter 
( 
(  
context  '
,' (
next) -
)- .
=>/ 1
{ 
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
LayersResource5 C
.C D
ConfiguredByHeaderD V
]V W
=X Y
$strZ d
;d e
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
$str5 G
]G H
=I J
$strK Q
;Q R
return 
next 
( 
context #
)# $
;$ %
} 
) 
; 
} 
} á
SD:\git\aur.archetypes.api\src\App.Api\Playground\Idempotency\IdempotencyResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Idempotency3 >
;> ?
internal 
sealed	 
class 
IdempotencyResource )
() *
)* +
:, -
PlaygroundResource. @
(@ A
$strA N
,N O
$strP ]
)] ^
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} ∑
]D:\git\aur.archetypes.api\src\App.Api\Playground\Idempotency\Endpoints\PostPaymentEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Idempotency3 >
.> ?
	Endpoints? H
;H I
internal

 
sealed

	 
class

 
PostPaymentEndpoint

 )
:

* +
MinimalEndpoint

, ;
<

; <
IdempotencyResource

< O
>

O P
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapPost 
( 
$str 
,  
Handle! '
)' (
. 
WithIdempotency 
( 
required %
:% &
true' +
)+ ,
;, -
static 
IResult 
Handle 
( 
HttpContext )
context* 1
)1 2
{ 	
var 
id 
= 
Guid 
. 
NewGuid !
(! "
)" #
;# $
context 
. 
Response 
. 
Headers $
[$ %
$str% 9
]9 :
=; <
$str= I
;I J
return 
TypedResults 
.  
Created  '
(' (
$"( *
$str* N
{N O
idO Q
}Q R
"R S
,S T
newU X
{Y Z
Id[ ]
=^ _
id` b
}c d
)d e
;e f
} 	
} 
} Ü
[D:\git\aur.archetypes.api\src\App.Api\Playground\Idempotency\Endpoints\PostOrderEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Idempotency3 >
.> ?
	Endpoints? H
;H I
internal

 
sealed

	 
class

 
PostOrderEndpoint

 '
:

( )
MinimalEndpoint

* 9
<

9 :
IdempotencyResource

: M
>

M N
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapPost 
( 
$str 
, 
(  
TimeProvider  ,
time- 1
)1 2
=>3 5
TypedResults6 B
.B C
OkC E
(E F
newF I
{J K
IdL N
=O P
GuidQ U
.U V
NewGuidV ]
(] ^
)^ _
,_ `
	CreatedAta j
=k l
timem q
.q r
	GetUtcNowr {
({ |
)| }
}~ 
)	 Ä
)
Ä Å
. 
WithIdempotency 
( 
) 
; 
} 
} √
XD:\git\aur.archetypes.api\src\App.Api\Playground\Diagnostics\Endpoints\GetLogEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Diagnostics3 >
.> ?
	Endpoints? H
;H I
internal		 
sealed			 
class		 
GetLogEndpoint		 $
:		% &
MinimalEndpoint		' 6
<		6 7
DiagnosticsResource		7 J
>		J K
{

 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str #
,# $
Handle% +
)+ ,
;, -
static 
IResult 
Handle 
( 
ILogger %
<% &
GetLogEndpoint& 4
>4 5
logger6 <
)< =
{ 	
logger 
. 
LogInformation !
(! "
$str" S
)S T
;T U
return 
TypedResults 
.  
	NoContent  )
() *
)* +
;+ ,
} 	
} 
} Ô	
ZD:\git\aur.archetypes.api\src\App.Api\Playground\Diagnostics\Endpoints\GetErrorEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Diagnostics3 >
.> ?
	Endpoints? H
;H I
internal		 
sealed			 
class		 
GetErrorEndpoint		 &
:		' (
MinimalEndpoint		) 8
<		8 9
DiagnosticsResource		9 L
>		L M
{

 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str %
,% &
Handle' -
)- .
;. /
static 
IResult 
Handle 
( 
) 
{ 	
throw 
new %
InvalidOperationException /
(/ 0
$str0 _
)_ `
;` a
} 	
} 
} ´
`D:\git\aur.archetypes.api\src\App.Api\Playground\Diagnostics\Endpoints\GetCorrelationEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Diagnostics		3 >
.		> ?
	Endpoints		? H
;		H I
internal 
sealed	 
class "
GetCorrelationEndpoint ,
:- .
MinimalEndpoint/ >
<> ?
DiagnosticsResource? R
>R S
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str +
,+ ,
Handle- 3
)3 4
;4 5
static 
IResult 
Handle 
( 
RequestContext ,
context- 4
)4 5
{ 	
return 
TypedResults 
.  
Ok  "
(" #
new# &
{ 
context 
. 
CorrelationId %
,% &
context 
. 
TraceId 
,  
context 
. 
OriginalSpanId &
,& '
context 
. 
ClientAddress %
,% &
context 
. 
	UserAgent !
,! "
context 
. 
Host 
, 
context 
. 
UserId 
, 
CurrentSpanId 
= 
Activity  (
.( )
Current) 0
?0 1
.1 2
SpanId2 8
.8 9
ToString9 A
(A B
)B C
,C D
Version 
= 

AppVersion $
.$ %
Current% ,
,, -
} 
) 
; 
} 	
}   
}!! Ü
_D:\git\aur.archetypes.api\src\App.Api\Playground\Diagnostics\Endpoints\GetAuthorizedEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Diagnostics3 >
.> ?
	Endpoints? H
;H I
internal

 
sealed

	 
class

 !
GetAuthorizedEndpoint

 +
:

, -
MinimalEndpoint

. =
<

= >
DiagnosticsResource

> Q
>

Q R
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str  
,  !
Handle" (
)( )
.  
RequireAuthorization !
(! "
)" #
;# $
static 
IResult 
Handle 
( 
ClaimsPrincipal -
user. 2
)2 3
{ 	
return 
TypedResults 
.  
Ok  "
(" #
new# &
{' (
Subject) 0
=1 2
user3 7
.7 8
	GetUserId8 A
(A B
)B C
,C D
userE I
.I J
IdentityJ R
?R S
.S T
NameT X
}Y Z
)Z [
;[ \
} 	
} 
} á
SD:\git\aur.archetypes.api\src\App.Api\Playground\Diagnostics\DiagnosticsResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Diagnostics3 >
;> ?
internal 
sealed	 
class 
DiagnosticsResource )
() *
)* +
:, -
PlaygroundResource. @
(@ A
$strA N
,N O
$strP ]
)] ^
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} ˇ
`D:\git\aur.archetypes.api\src\App.Api\Playground\Dependencies\Endpoints\GetRandomUserEndpoint.cs
	namespace

 	
Aurore


 
.

 

Archetypes

 
.

 
Api

 
.

  
App

  #
.

# $
Api

$ '
.

' (

Playground

( 2
.

2 3
Dependencies

3 ?
.

? @
	Endpoints

@ I
;

I J
internal 
sealed	 
class !
GetRandomUserEndpoint +
:, -
MinimalEndpoint. =
<= > 
DependenciesResource> R
>R S
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str +
,+ ,
HandleAsync- 8
)8 9
;9 :
static 
async 
Task 
< 
IResult !
>! "
HandleAsync# .
(. /
IHttpClientFactory/ A
clientFactoryB O
,O P
CancellationTokenQ b
cancellationTokenc t
)t u
{ 	
var 
http 
= 
clientFactory $
.$ %
CreateClient% 1
(1 2
HttpDependencyNames2 E
.E F

RandomUserF P
)P Q
;Q R
using 
var 
response 
=  
await! &
http' +
.+ ,
GetAsync, 4
(4 5
$str5 E
,E F
cancellationTokenG X
)X Y
;Y Z
var 
body 
= 
await 
response %
.% &
Content& -
.- .
ReadAsStringAsync. ?
(? @
cancellationToken@ Q
)Q R
;R S
return 
TypedResults 
.  
Content  '
(' (
body( ,
,, -
$str. @
,@ A

statusCodeB L
:L M
(N O
intO R
)R S
responseS [
.[ \

StatusCode\ f
)f g
;g h
} 	
} 
} †
`D:\git\aur.archetypes.api\src\App.Api\Playground\Dependencies\Endpoints\GetGitHubUserEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Dependencies		3 ?
.		? @
	Endpoints		@ I
;		I J
internal 
sealed	 
class !
GetGitHubUserEndpoint +
:, -
MinimalEndpoint. =
<= > 
DependenciesResource> R
>R S
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str -
,- .
HandleAsync/ :
): ;
;; <
static 
async 
Task 
< 
IResult !
>! "
HandleAsync# .
(. /
string/ 5
user6 :
,: ;
IGitHubClient< I
gitHubJ P
,P Q
CancellationTokenR c
cancellationTokend u
)u v
{ 	
var 
result 
= 
await 
gitHub %
.% &
GetUserAsync& 2
(2 3
user3 7
,7 8
cancellationToken9 J
)J K
;K L
return 
result 
is 
null !
?" #
TypedResults$ 0
.0 1
NotFound1 9
(9 :
): ;
:< =
TypedResults> J
.J K
OkK M
(M N
resultN T
)T U
;U V
} 	
} 
} ã
UD:\git\aur.archetypes.api\src\App.Api\Playground\Dependencies\DependenciesResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Dependencies3 ?
;? @
internal 
sealed	 
class  
DependenciesResource *
(* +
)+ ,
:- .
PlaygroundResource/ A
(A B
$strB P
,P Q
$strR i
)i j
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} à
ZD:\git\aur.archetypes.api\src\App.Api\Playground\Caching\Endpoints\GetTimestampEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Caching3 :
.: ;
	Endpoints; D
;D E
internal

 
sealed

	 
class

  
GetTimestampEndpoint

 *
:

+ ,
MinimalEndpoint

- <
<

< =
CachingResource

= L
>

L M
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
,  
(! "
TimeProvider" .
time/ 3
)3 4
=>5 7
TypedResults8 D
.D E
OkE G
(G H
newH K
{L M
	TimestampN W
=X Y
timeZ ^
.^ _
	GetUtcNow_ h
(h i
)i j
}k l
)l m
)m n
. 
CacheOutput 
( 
$str  
)  !
;! "
} 
} ˜
KD:\git\aur.archetypes.api\src\App.Api\Playground\Caching\CachingResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Caching3 :
;: ;
internal 
sealed	 
class 
CachingResource %
(% &
)& '
:( )
PlaygroundResource* <
(< =
$str= F
,F G
$strH X
)X Y
{ 
public 

override 
bool  
RequireAuthorization -
=>. 0
false1 6
;6 7
} ”

SD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetScopeEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Auth3 7
.7 8
	Endpoints8 A
;A B
internal

 
sealed

	 
class

 
GetScopeEndpoint

 &
:

' (
MinimalEndpoint

) 8
<

8 9
AuthResource

9 E
>

E F
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
( 
) 
=>  "
TypedResults# /
./ 0
Ok0 2
(2 3
new3 6
{7 8
Granted9 @
=A B
	AppScopesC L
.L M
ReadM Q
}R S
)S T
)T U
. 
RequireScopes 
( 
	AppScopes $
.$ %
Read% )
)) *
;* +
} 
} Ñ

RD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetRoleEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Auth3 7
.7 8
	Endpoints8 A
;A B
internal		 
sealed			 
class		 
GetRoleEndpoint		 %
:		& '
MinimalEndpoint		( 7
<		7 8
AuthResource		8 D
>		D E
{

 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
( 
) 
=> !
TypedResults" .
.. /
Ok/ 1
(1 2
new2 5
{6 7
Granted8 ?
=@ A
$strB I
}J K
)K L
)L M
. 
RequireRoles 
( 
$str !
)! "
;" #
} 
} Á
ZD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetEitherSchemeEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Auth		3 7
.		7 8
	Endpoints		8 A
;		A B
internal 
sealed	 
class #
GetEitherSchemeEndpoint -
:. /
MinimalEndpoint0 ?
<? @
AuthResource@ L
>L M
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str &
,& '
Handle( .
). /
./ 0
RequireScopes0 =
(= >
	AppScopes> G
.G H
ReadH L
)L M
;M N
static 
IResult 
Handle 
( 
ClaimsPrincipal -
user. 2
)2 3
{ 	
return 
TypedResults 
.  
Ok  "
(" #
new# &
{' (
Caller) /
=0 1
user2 6
.6 7
Identity7 ?
?? @
.@ A
NameA E
,E F
SchemeG M
=N O
userP T
.T U
IdentityU ]
?] ^
.^ _
AuthenticationType_ q
}r s
)s t
;t u
} 	
} 
} ç
VD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetCombinedEndpoint.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Auth3 7
.7 8
	Endpoints8 A
;A B
internal

 
sealed

	 
class

 
GetCombinedEndpoint

 )
:

* +
MinimalEndpoint

, ;
<

; <
AuthResource

< H
>

H I
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str 
, 
(  !
)! "
=># %
TypedResults& 2
.2 3
Ok3 5
(5 6
new6 9
{: ;
Granted< C
=D E
$strF Y
}Z [
)[ \
)\ ]
. 
RequireScopes 
( 
	AppScopes $
.$ %
Write% *
)* +
. 
RequireRoles 
( 
$str !
)! "
;" #
} 
} ß
XD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetBearerOnlyEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Auth		3 7
.		7 8
	Endpoints		8 A
;		A B
internal 
sealed	 
class !
GetBearerOnlyEndpoint +
:, -
MinimalEndpoint. =
<= >
AuthResource> J
>J K
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str !
,! "
(# $
ClaimsPrincipal$ 3
user4 8
)8 9
=>: <
TypedResults= I
.I J
OkJ L
(L M
newM P
{Q R
SubjectS Z
=[ \
user] a
.a b
	GetUserIdb k
(k l
)l m
}n o
)o p
)p q
.  
RequireAuthorization !
(! "
policy" (
=>) +
policy, 2
. $
AddAuthenticationSchemes )
() *
JwtBearerDefaults* ;
.; < 
AuthenticationScheme< P
)P Q
. $
RequireAuthenticatedUser )
() *
)* +
)+ ,
;, -
} 
} ß
[D:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetAuthenticatedEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Auth		3 7
.		7 8
	Endpoints		8 A
;		A B
internal 
sealed	 
class $
GetAuthenticatedEndpoint .
:/ 0
MinimalEndpoint1 @
<@ A
AuthResourceA M
>M N
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str -
,- .
Handle/ 5
)5 6
;6 7
static 
IResult 
Handle 
( 
ClaimsPrincipal -
user. 2
)2 3
{ 	
return 
TypedResults 
.  
Ok  "
(" #
new# &
{ 
Subject 
= 
user 
. 
	GetUserId (
(( )
)) *
,* +
Claims 
= 
user 
. 
Claims $
.$ %
Select% +
(+ ,
x, -
=>. 0
new1 4
{5 6
x7 8
.8 9
Type9 =
,= >
x? @
.@ A
ValueA F
}G H
)H I
,I J
} 
) 
; 
} 	
} 
} î
XD:\git\aur.archetypes.api\src\App.Api\Playground\Auth\Endpoints\GetApiKeyOnlyEndpoint.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (

Playground		( 2
.		2 3
Auth		3 7
.		7 8
	Endpoints		8 A
;		A B
internal 
sealed	 
class !
GetApiKeyOnlyEndpoint +
:, -
MinimalEndpoint. =
<= >
AuthResource> J
>J K
{ 
public 

override 
RouteHandlerBuilder '
Map( +
(+ ,!
IEndpointRouteBuilder, A
builderB I
)I J
{ 
return 
builder 
. 
MapGet 
( 
$str "
," #
($ %
ClaimsPrincipal% 4
user5 9
)9 :
=>; =
TypedResults> J
.J K
OkK M
(M N
newN Q
{R S
ClientT Z
=[ \
user] a
.a b
Identityb j
?j k
.k l
Namel p
}q r
)r s
)s t
.  
RequireAuthorization !
(! "
policy" (
=>) +
policy, 2
.2 3$
AddAuthenticationSchemes3 K
(K L
ApiKeyDefaultsL Z
.Z [ 
AuthenticationScheme[ o
)o p
.p q%
RequireAuthenticatedUser	q â
(
â ä
)
ä ã
)
ã å
. 
RequireRoles 
( 
$str "
)" #
;# $
} 
} ô
ND:\git\aur.archetypes.api\src\App.Api\Configuration\ValidationConfiguration.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
App		  #
.		# $
Api		$ '
.		' (
Configuration		( 5
;		5 6
internal 
static	 
class #
ValidationConfiguration -
{ 
internal 
static 
IServiceCollection &
AddAppValidators' 7
(7 8
this8 <
IServiceCollection= O
servicesP X
,X Y
AppSettingsZ e
appSettingsf q
)q r
{ 
ValidatorOptions 
. 
Global 
.  
LanguageManager  /
./ 0
Culture0 7
=8 9
new: =
CultureInfo> I
(I J
appSettingsJ U
.U V
AppInformationV d
.d e
Localee k
)k l
;l m
return 
services 
. !
AddStandardValidators -
(- .
[ 
typeof 
( 
Program 
) 
. 
Assembly %
,% &
typeof' -
(- .
ICore. 3
)3 4
.4 5
Assembly5 =
,= >
typeof? E
(E F
	IFeaturesF O
)O P
.P Q
AssemblyQ Y
]Y Z
,Z [ 
includeInternalTypes  
:  !
true" &
)& '
;' (
} 
} €
ED:\git\aur.archetypes.api\src\App.Api\Playground\Auth\AuthResource.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (

Playground( 2
.2 3
Auth3 7
;7 8
internal 
sealed	 
class 
AuthResource "
(" #
)# $
:% &
PlaygroundResource' 9
(9 :
$str: @
,@ A
$strB Q
)Q R
{ 
} î#
JD:\git\aur.archetypes.api\src\App.Api\Configuration\ScalarConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
Configuration( 5
;5 6
internal 
static	 
class 
ScalarConfiguration )
{ 
internal 
static #
IHostApplicationBuilder +
AddAppScalar, 8
(8 9
this #
IHostApplicationBuilder $
builder% ,
,, -
AppSettings 
appSettings 
,  "
MinimalEndpointCatalog 
	endpoints (
)( )
{ 
if 

( 
builder 
. 
Environment 
.  
IsProduction  ,
(, -
)- .
). /
return 
builder 
; 
builder 
. ,
 ConfigureStandardScalarDocuments 0
(0 1
	endpoints1 :
.: ;
Versions; C
)C D
;D E
foreach 
( 
var 
version 
in 
	endpoints  )
.) *
Versions* 2
)2 3
{ 	
builder 
. 
Services 
. 

AddOpenApi 
( 
OpenApiExtensions -
.- .
DocumentName. :
(: ;
version; B
.B C
NumberC I
)I J
,J K
optionsL S
=>T V
optionsW ^
. #
AddStandardDocumentInfo ,
(, -
version- 4
,4 5
appSettings6 A
.A B
AppInformationB P
)P Q
. (
AddStandardJwtBearerSecurity 1
(1 2
)2 3
. %
AddStandardApiKeySecurity .
(. /
appSettings/ :
.: ;
ApiKeys; B
)B C
.   #
AddStandardOidcSecurity   ,
(  , -
appSettings  - 8
.  8 9
IdentityProvider  9 I
)  I J
.!! %
AddStandardOAuth2Security!! .
(!!. /
appSettings!!/ :
.!!: ;
IdentityProvider!!; K
)!!K L
."" /
#AddStandardCamelCaseQueryParameters"" 8
(""8 9
)""9 :
.## *
AddStandardCorrelationIdHeader## 3
(##3 4
)##4 5
.$$ +
AddStandardIdempotencyKeyHeader$$ 4
($$4 5
)$$5 6
)$$6 7
;$$7 8
}%% 	
return'' 
builder'' 
;'' 
}(( 
internal** 
static** 
WebApplication** "
MapAppScalar**# /
(**/ 0
this**0 4
WebApplication**5 C
app**D G
,**G H
AppSettings**I T
appSettings**U `
)**` a
{++ 
if,, 

(,, 
app,, 
.,, 
Environment,, 
.,, 
IsProduction,, (
(,,( )
),,) *
),,* +
return-- 
app-- 
;-- 
app// 
.// 

MapOpenApi// 
(// 
)// 
;// 
app11 
.11 !
MapScalarApiReference11 !
(11! "
options11" )
=>11* ,
options11- 4
.22 
	WithTitle22 
(22 
appSettings22 "
.22" #
AppInformation22# 1
.221 2
	Component222 ;
.22; <
ToTitleCase22< G
(22G H
)22H I
)22I J
.33 
	WithTheme33 
(33 
ScalarTheme33 "
.33" #
Default33# *
)33* +
.44 !
WithDefaultHttpClient44 "
(44" #
ScalarTarget44# /
.44/ 0
Shell440 5
,445 6
ScalarClient447 C
.44C D
Curl44D H
)44H I
.55 &
WithStandardAuthentication55 '
(55' (
appSettings55( 3
.553 4
IdentityProvider554 D
,55D E
appSettings55F Q
.55Q R
ApiDocs55R Y
)55Y Z
)55Z [
;55[ \
return77 
app77 
;77 
}88 
}99 ±
MD:\git\aur.archetypes.api\src\App.Api\Configuration\ResourcesConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
Configuration( 5
;5 6
internal		 
static			 
class		 "
ResourcesConfiguration		 ,
{

 
internal 
static 
WebApplication "
MapAppResources# 2
(2 3
this3 7
WebApplication8 F
appG J
,J K"
MinimalEndpointCatalogL b
	endpointsc l
)l m
{ 
app 
. '
MapStandardMinimalEndpoints '
(' (
	endpoints( 1
,1 2
api3 6
=>7 9
api: =
. 
ProducesProblem 
( 
StatusCodes (
.( )'
Status503ServiceUnavailable) D
)D E
. 
AddEndpointFilter 
( 
(  
context  '
,' (
next) -
)- .
=>/ 1
{ 
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
LayersResource5 C
.C D
ConfiguredByHeaderD V
]V W
=X Y
$strZ _
;_ `
context 
. 
HttpContext #
.# $
Response$ ,
., -
Headers- 4
[4 5
$str5 B
]B C
=D E
$strF L
;L M
return 
next 
( 
context #
)# $
;$ %
} 
) 
) 
; 
return 
app 
; 
} 
} •

OD:\git\aur.archetypes.api\src\App.Api\Configuration\AppSettingsConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
App  #
.# $
Api$ '
.' (
Configuration( 5
;5 6
internal		 
static			 
class		 $
AppSettingsConfiguration		 .
{

 
internal 
static #
IHostApplicationBuilder +
LoadAppSettings, ;
(; <
this #
IHostApplicationBuilder $
builder% ,
,, -
out 
AppSettings 
appSettings #
)# $
{ 
appSettings 
= 
builder 
. 
Configuration +
.+ ,
	GetStrict, 5
<5 6
AppSettings6 A
>A B
(B C
)C D
;D E
new  
AppSettingsValidator  
(  !
builder! (
.( )
Environment) 4
)4 5
.5 6
ValidateAndThrow6 F
(F G
appSettingsG R
)R S
;S T
return 
builder 
; 
} 
} 