ú
-D:\git\aur.archetypes.api\src\Infra\IInfra.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
;% &
public 
	interface 
IInfra 
; ∑
PD:\git\aur.archetypes.api\src\Infra\Contexts\Configurations\PostConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Contexts& .
.. /
Configurations/ =
;= >
internal		 
sealed			 
class		 
PostConfiguration		 '
:		( )#
EntityConfigurationBase		* A
<		A B
Post		B F
>		F G
{

 
public 

const 
string 
UniqueSlugIndex '
=( )
$str* 9
;9 :
	protected 
override 
void 
Extend "
(" #
EntityTypeBuilder# 4
<4 5
Post5 9
>9 :
builder; B
)B C
{ 
builder 
. 
Property 
( 
x 
=> 
x 
.  
Slug  $
)$ %
.% &
HasMaxLength& 2
(2 3
	BlogRules3 <
.< =
SlugMaxLength= J
)J K
;K L
builder 
. 
Property 
( 
x 
=> 
x 
.  
Title  %
)% &
.& '
HasMaxLength' 3
(3 4
	BlogRules4 =
.= >
TitleMaxLength> L
)L M
;M N
builder 
. 
Property 
( 
x 
=> 
x 
.  
Body  $
)$ %
.% &
HasMaxLength& 2
(2 3
	BlogRules3 <
.< =
PostBodyMaxLength= N
)N O
;O P
builder 
. 
Property 
( 
x 
=> 
x 
.  
Status  &
)& '
.' (
HasConversion( 5
<5 6
string6 <
>< =
(= >
)> ?
.? @
HasMaxLength@ L
(L M
	BlogRulesM V
.V W
StatusMaxLengthW f
)f g
;g h
builder 
. 
HasIndex 
( 
x 
=> 
x 
.  
Slug  $
)$ %
.% &
IsUnique& .
(. /
)/ 0
.0 1
HasDatabaseName1 @
(@ A
UniqueSlugIndexA P
)P Q
;Q R
builder 
. 
HasIndex 
( 
x 
=> 
new !
{" #
x$ %
.% &
Status& ,
,, -
x. /
./ 0
PublishedAt0 ;
}< =
)= >
;> ?
builder 
. 
HasOne 
( 
x 
=> 
x 
. 
Author !
)! "
. 
WithMany 
( 
) 
. 
HasForeignKey 
( 
x 
=> 
x  !
.! "
AuthorId" *
)* +
. 
OnDelete 
( 
DeleteBehavior $
.$ %
Restrict% -
)- .
;. /
} 
} ô
YD:\git\aur.archetypes.api\src\Infra\Contexts\Configurations\CommentReportConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Contexts& .
.. /
Configurations/ =
;= >
internal		 
sealed			 
class		 &
CommentReportConfiguration		 0
:		1 2#
EntityConfigurationBase		3 J
<		J K
CommentReport		K X
>		X Y
{

 
public 

const 
string 
UniqueReporterIndex +
=, -
$str. S
;S T
	protected 
override 
void 
Extend "
(" #
EntityTypeBuilder# 4
<4 5
CommentReport5 B
>B C
builderD K
)K L
{ 
builder 
. 
Property 
( 
x 
=> 
x 
.  
Reason  &
)& '
.' (
HasMaxLength( 4
(4 5
	BlogRules5 >
.> ?!
ReportReasonMaxLength? T
)T U
;U V
builder 
. 
HasIndex 
( 
x 
=> 
new !
{" #
x$ %
.% &
	CommentId& /
,/ 0
x1 2
.2 3

ReporterId3 =
}> ?
)? @
.@ A
IsUniqueA I
(I J
)J K
.K L
HasDatabaseNameL [
([ \
UniqueReporterIndex\ o
)o p
;p q
builder 
. 
HasOne 
( 
x 
=> 
x 
. 
Comment "
)" #
. 
WithMany 
( 
) 
. 
HasForeignKey 
( 
x 
=> 
x  !
.! "
	CommentId" +
)+ ,
. 
OnDelete 
( 
DeleteBehavior $
.$ %
Restrict% -
)- .
;. /
builder 
. 
HasOne 
( 
x 
=> 
x 
. 
Reporter #
)# $
. 
WithMany 
( 
) 
. 
HasForeignKey 
( 
x 
=> 
x  !
.! "

ReporterId" ,
), -
. 
OnDelete 
( 
DeleteBehavior $
.$ %
Restrict% -
)- .
;. /
} 
}   â
SD:\git\aur.archetypes.api\src\Infra\Contexts\Configurations\CommentConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Contexts& .
.. /
Configurations/ =
;= >
internal		 
sealed			 
class		  
CommentConfiguration		 *
:		+ ,#
EntityConfigurationBase		- D
<		D E
Comment		E L
>		L M
{

 
	protected 
override 
void 
Extend "
(" #
EntityTypeBuilder# 4
<4 5
Comment5 <
>< =
builder> E
)E F
{ 
builder 
. 
Property 
( 
x 
=> 
x 
.  
Body  $
)$ %
.% &
HasMaxLength& 2
(2 3
	BlogRules3 <
.< = 
CommentBodyMaxLength= Q
)Q R
;R S
builder 
. 
HasIndex 
( 
x 
=> 
new !
{" #
x$ %
.% &
PostId& ,
,, -
x. /
./ 0
	CreatedAt0 9
}: ;
); <
;< =
builder 
. 
HasOne 
( 
x 
=> 
x 
. 
Post 
)  
. 
WithMany 
( 
x 
=> 
x 
. 
Comments %
)% &
. 
HasForeignKey 
( 
x 
=> 
x  !
.! "
PostId" (
)( )
. 
OnDelete 
( 
DeleteBehavior $
.$ %
Restrict% -
)- .
;. /
builder 
. 
HasOne 
( 
x 
=> 
x 
. 
Author !
)! "
. 
WithMany 
( 
) 
. 
HasForeignKey 
( 
x 
=> 
x  !
.! "
AuthorId" *
)* +
. 
OnDelete 
( 
DeleteBehavior $
.$ %
Restrict% -
)- .
;. /
} 
} «
RD:\git\aur.archetypes.api\src\Infra\Contexts\Configurations\AuthorConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Contexts& .
.. /
Configurations/ =
;= >
internal		 
sealed			 
class		 
AuthorConfiguration		 )
:		* +#
EntityConfigurationBase		, C
<		C D
Author		D J
>		J K
{

 
public 

const 
string 
UniqueSubjectIndex *
=+ ,
$str- A
;A B
public 

const 
string 
UniqueHandleIndex )
=* +
$str, ?
;? @
	protected 
override 
void 
Extend "
(" #
EntityTypeBuilder# 4
<4 5
Author5 ;
>; <
builder= D
)D E
{ 
builder 
. 
Property 
( 
x 
=> 
x 
.  
Subject  '
)' (
.( )
HasMaxLength) 5
(5 6
	BlogRules6 ?
.? @
SubjectMaxLength@ P
)P Q
;Q R
builder 
. 
Property 
( 
x 
=> 
x 
.  
Handle  &
)& '
.' (
HasMaxLength( 4
(4 5
	BlogRules5 >
.> ?
HandleMaxLength? N
)N O
;O P
builder 
. 
Property 
( 
x 
=> 
x 
.  
DisplayName  +
)+ ,
., -
HasMaxLength- 9
(9 :
	BlogRules: C
.C D 
DisplayNameMaxLengthD X
)X Y
;Y Z
builder 
. 
HasIndex 
( 
x 
=> 
x 
.  
Subject  '
)' (
.( )
IsUnique) 1
(1 2
)2 3
.3 4
HasDatabaseName4 C
(C D
UniqueSubjectIndexD V
)V W
;W X
builder 
. 
HasIndex 
( 
x 
=> 
x 
.  
Handle  &
)& '
.' (
IsUnique( 0
(0 1
)1 2
.2 3
HasDatabaseName3 B
(B C
UniqueHandleIndexC T
)T U
;U V
} 
} ™
<D:\git\aur.archetypes.api\src\Infra\Contexts\AppDbContext.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Contexts& .
;. /
public 
sealed 
class 
AppDbContext  
(  !
DbContextOptions! 1
<1 2
AppDbContext2 >
>> ?
options@ G
)G H
:I J
	DbContextK T
(T U
optionsU \
)\ ]
{ 
public		 

DbSet		 
<		 
Author		 
>		 
Authors		  
=>		! #
Set		$ '
<		' (
Author		( .
>		. /
(		/ 0
)		0 1
;		1 2
public 

DbSet 
< 
Post 
> 
Posts 
=> 
Set  #
<# $
Post$ (
>( )
() *
)* +
;+ ,
public 

DbSet 
< 
Comment 
> 
Comments "
=># %
Set& )
<) *
Comment* 1
>1 2
(2 3
)3 4
;4 5
public 

DbSet 
< 
CommentReport 
> 
CommentReports  .
=>/ 1
Set2 5
<5 6
CommentReport6 C
>C D
(D E
)E F
;F G
	protected 
override 
void 
OnModelCreating +
(+ ,
ModelBuilder, 8
modelBuilder9 E
)E F
{ 
modelBuilder 
. +
ApplyConfigurationsFromAssembly ,
(, -
typeof- 3
(3 4
IInfra4 :
): ;
.; <
Assembly< D
)D E
. $
ApplyStandardConventions %
(% &
)& '
;' (
} 
} É,
GD:\git\aur.archetypes.api\src\Infra\Configuration\InfraConfiguration.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Configuration& 3
;3 4
public 
static 
class 
InfraConfiguration &
{ 
public 

static 
IServiceCollection $
AddAppInfra% 0
(0 1
this1 5
IServiceCollection6 H
servicesI Q
,Q R
AppSettingsS ^
appSettings_ j
)j k
{ 
services 
. 
AddAppDatabase 
(  
appSettings  +
.+ ,
Postgres, 4
)4 5
;5 6
services 
. 
AddSingleton 
( 
appSettings )
.) *
Email* /
)/ 0
;0 1
services 
. 
AddSingleton 
< 
IEmailSender *
,* +
SmtpEmailSender, ;
>; <
(< =
)= >
;> ?
services 
. "
AddPostgresHealthCheck '
(' (
appSettings( 3
.3 4
Postgres4 <
)< =
;= >
return 
services 
. "
AddAppHttpDependencies .
(. /
appSettings/ :
): ;
;; <
} 
public!! 

static!! 
IServiceCollection!! $
AddAppDatabase!!% 3
(!!3 4
this!!4 8
IServiceCollection!!9 K
services!!L T
,!!T U
PostgresOptions!!V e
postgres!!f n
)!!n o
{"" 
return## 
services## 
.$$  
AddStandardDbContext$$ !
<$$! "
AppDbContext$$" .
>$$. /
($$/ 0
postgres$$0 8
)$$8 9
.%% $
AddUniqueViolationErrors%% %
(%%% &
map%%& )
=>%%* ,
map%%- 0
.&& 
On&& 
(&& 
AuthorConfiguration&& '
.&&' (
UniqueHandleIndex&&( 9
,&&9 :
	AppErrors&&; D
.&&D E
Authors&&E L
.&&L M
HandleTaken&&M X
)&&X Y
.'' 
On'' 
('' &
CommentReportConfiguration'' .
.''. /
UniqueReporterIndex''/ B
,''B C
	AppErrors''D M
.''M N
Comments''N V
.''V W
AlreadyReported''W f
)''f g
)''g h
;''h i
}(( 
private** 
static** 
IServiceCollection** %"
AddAppHttpDependencies**& <
(**< =
this**= A
IServiceCollection**B T
services**U ]
,**] ^
AppSettings**_ j
appSettings**k v
)**v w
{++ 
var,, 
dependencies,, 
=,, 
appSettings,, &
.,,& '
HttpDependencies,,' 7
;,,7 8
services// 
.// %
AddStandardHttpDependency// *
<//* +
IGitHubClient//+ 8
,//8 9
GitHubClient//: F
>//F G
(//G H
HttpDependencyNames//H [
.//[ \
GitHub//\ b
,//b c
dependencies//d p
.//p q
GetRequired//q |
(//| } 
HttpDependencyNames	//} ê
.
//ê ë
GitHub
//ë ó
)
//ó ò
)
//ò ô
;
//ô ö
services22 
.22 
AddTransient22 
<22 $
RandomUserSigningHandler22 6
>226 7
(227 8
)228 9
;229 :
services33 
.44 %
AddStandardHttpDependency44 &
(44& '
HttpDependencyNames44' :
.44: ;

RandomUser44; E
,44E F
dependencies44G S
.44S T
GetRequired44T _
(44_ `
HttpDependencyNames44` s
.44s t

RandomUser44t ~
)44~ 
)	44 Ä
.55 !
AddHttpMessageHandler55 "
<55" #$
RandomUserSigningHandler55# ;
>55; <
(55< =
)55= >
;55> ?
var88 
orders88 
=88 
dependencies88 !
.88! "
GetRequired88" -
(88- .
HttpDependencyNames88. A
.88A B
Orders88B H
)88H I
;88I J
services99 
.:: %
AddStandardHttpDependency:: &
(::& '
HttpDependencyNames::' :
.::: ;
Orders::; A
,::A B
orders::C I
)::I J
.;; (
AddStandardClientCredentials;; )
(;;) *
orders;;* 0
.;;0 1
ClientCredentials;;1 B
);;B C
;;;C D
var>> 
	inventory>> 
=>> 
dependencies>> $
.>>$ %
GetRequired>>% 0
(>>0 1
HttpDependencyNames>>1 D
.>>D E
	Inventory>>E N
)>>N O
;>>O P
services?? 
.@@ %
AddStandardHttpDependency@@ &
(@@& '
HttpDependencyNames@@' :
.@@: ;
	Inventory@@; D
,@@D E
	inventory@@F O
)@@O P
.AA 
AddStandardApiKeyAA 
(AA 
	inventoryAA (
.AA( )
ApiKeyAA) /
)AA/ 0
;AA0 1
returnCC 
servicesCC 
;CC 
}DD 
}EE ı
RD:\git\aur.archetypes.api\src\Infra\Clients\RandomUser\RandomUserSigningHandler.cs
	namespace		 	
Aurore		
 
.		 

Archetypes		 
.		 
Api		 
.		  
Infra		  %
.		% &
Clients		& -
.		- .

RandomUser		. 8
;		8 9
internal 
sealed	 
class $
RandomUserSigningHandler .
(. /
TimeProvider/ ;
timeProvider< H
)H I
:J K
DelegatingHandlerL ]
{ 
private 
static 
readonly 
byte  
[  !
]! "
_signingKey# .
=/ 0
Encoding1 9
.9 :
UTF8: >
.> ?
GetBytes? G
(G H
$strH Z
)Z [
;[ \
	protected 
override 
Task 
< 
HttpResponseMessage /
>/ 0
	SendAsync1 :
(: ;
HttpRequestMessage 
request "
," #
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
	timestamp 
= 
timeProvider $
.$ %
	GetUtcNow% .
(. /
)/ 0
.0 1
ToUnixTimeSeconds1 B
(B C
)C D
.D E
ToStringE M
(M N
CultureInfoN Y
.Y Z
InvariantCultureZ j
)j k
;k l
var 
payload 
= 
$" 
{ 
request  
.  !
Method! '
}' (
$str( *
{* +
request+ 2
.2 3

RequestUri3 =
?= >
.> ?
PathAndQuery? K
}K L
$strL N
{N O
	timestampO X
}X Y
"Y Z
;Z [
var 
	signature 
= 
Convert 
.  
ToHexStringLower  0
(0 1

HMACSHA2561 ;
.; <
HashData< D
(D E
_signingKeyE P
,P Q
EncodingR Z
.Z [
UTF8[ _
._ `
GetBytes` h
(h i
payloadi p
)p q
)q r
)r s
;s t
request 
. 
Headers 
. #
TryAddWithoutValidation /
(/ 0
$str0 ?
,? @
	timestampA J
)J K
;K L
request 
. 
Headers 
. #
TryAddWithoutValidation /
(/ 0
$str0 ?
,? @
	signatureA J
)J K
;K L
return 
base 
. 
	SendAsync 
( 
request %
,% &
cancellationToken' 8
)8 9
;9 :
} 
} …
BD:\git\aur.archetypes.api\src\Infra\Clients\GitHub\GitHubClient.cs
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
  
Infra

  %
.

% &
Clients

& -
.

- .
GitHub

. 4
;

4 5
internal 
sealed	 
class 
GitHubClient "
(" #

HttpClient# -
http. 2
)2 3
:4 5
IGitHubClient6 C
{ 
private 
static 
readonly !
JsonSerializerOptions 1
_json2 7
=8 9
new: =
(= >"
JsonSerializerDefaults> T
.T U
WebU X
)X Y
{  
PropertyNamingPolicy 
= 
JsonNamingPolicy /
./ 0
SnakeCaseLower0 >
,> ?
} 
; 
public 

async 
Task 
< 

GitHubUser  
?  !
>! "
GetUserAsync# /
(/ 0
string0 6
login7 <
,< =
CancellationToken> O
cancellationTokenP a
)a b
{ 
using 
var 
response 
= 
await "
http# '
.' (
GetAsync( 0
(0 1
$"1 3
$str3 9
{9 :
Uri: =
.= >
EscapeDataString> N
(N O
loginO T
)T U
}U V
"V W
,W X
cancellationTokenY j
)j k
;k l
if 

( 
response 
. 

StatusCode 
is  "
HttpStatusCode# 1
.1 2
NotFound2 :
): ;
return 
null 
; 
response 
. #
EnsureSuccessStatusCode (
(( )
)) *
;* +
return 
await 
response 
. 
Content %
.% &
ReadFromJsonAsync& 7
<7 8

GitHubUser8 B
>B C
(C D
_jsonD I
,I J
cancellationTokenK \
)\ ]
;] ^
} 
} ¸
DD:\git\aur.archetypes.api\src\Infra\Clients\Email\SmtpEmailSender.cs
	namespace 	
Aurore
 
. 

Archetypes 
. 
Api 
.  
Infra  %
.% &
Clients& -
.- .
Email. 3
;3 4
internal 
sealed	 
class 
SmtpEmailSender %
(% &
EmailOptions& 2
options3 :
): ;
:< =
IEmailSender> J
{ 
[ 
SuppressMessage 
( 
$str -
,- .
$str/ g
,g h
Justificationi v
=w x
$str	y –
)
– —
]
— “
public 

async 
Task 
	SendAsync 
(  
EmailMessage  ,
message- 4
,4 5
CancellationToken6 G
cancellationTokenH Y
)Y Z
{ 
using 
var 
mail 
= 
new 
MailMessage (
{ 	
From 
= 
new 
MailAddress "
(" #
options# *
.* +
From+ /
)/ 0
,0 1
Subject 
= 
message 
. 
Subject %
,% &
Body 
= 
message 
. 
Body 
,  

IsBodyHtml 
= 
false 
, 
} 	
;	 

foreach 
( 
var 
address 
in 
message  '
.' (
To( *
.* +
Distinct+ 3
(3 4
)4 5
)5 6
mail 
. 
To 
. 
Add 
( 
address 
)  
;  !
using 
var 
client 
= 
new 

SmtpClient )
() *
options* 1
.1 2
Host2 6
,6 7
options8 ?
.? @
Port@ D
)D E
{F G
	EnableSslH Q
=R S
optionsT [
.[ \
	EnableSsl\ e
}f g
;g h
if 

( 
options 
. 
Username 
. 
HasValue %
(% &
)& '
)' (
client   
.   
Credentials   
=    
new  ! $
NetworkCredential  % 6
(  6 7
options  7 >
.  > ?
Username  ? G
,  G H
options  I P
.  P Q
Password  Q Y
)  Y Z
;  Z [
await"" 
client"" 
."" 
SendMailAsync"" "
(""" #
mail""# '
,""' (
cancellationToken"") :
)"": ;
;""; <
}## 
}$$ 