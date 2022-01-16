using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalenTrack.Resources {
	class DefaultConfig {
		public static string get() {
			return @"; this is not a by-spec INI file
[main]
scale=6
timeTillIdle=60
idleAlpha=0.7
hourStyle=24
timeTillBreak=0
breakTime=180
breakText=Take a break!
breakTaskbarKind=error
autosaveInterval=300

[colors]
section=#000000
border=#000000
selected=#FFFFFF
rest=rgb(60,170,120)
unknown=rgb(120,60,170)
amiss=rgba(0,0,0,0)
marker=#00FFFF
;auto=rgb(60,170,75)
; you can use $vars you declare here as color below:
$dev=rgb(250,50,50)
$web=rgb(250,200,50)
$comm=#B146C2

[rule]
titleHas=Visual Studio Code
title=^.+? - (.+?) - Visual Studio Code$
color=$dev
label=$t1 [VSCode]

[rule]
titleHas=Visual Studio Code
title=^.+? - Visual Studio Code$
label=??? [VSCode]
color=$dev

[rule]
titleHas=Microsoft Visual Studio
title=^(.+?) - Microsoft Visual Studio.*$
label=$t1 [VS]
color=$dev

[rule]
pathHas=Discord
path=^.+\\Discord(?:Canary)?\.exe$
color=#6D87FF

[rule]
pathHas=\chrome.exe
path=^.+\\chrome\.exe$
color=$web
[rule]
pathHas=\msedge.exe
path=^.+\\msedge\.exe$
color=$web
[rule]
pathHas=\firefox.exe
path=^.+\\firefox\.exe$
color=$web
";
		}
	}
}
