# TTConsole

The Best Debuger &amp; Runtime Console For Unity3D

![image](https://github.com/chiuan/TTConsole/blob/master/UI.png)
![image](https://github.com/chiuan/TTConsole/blob/master/runtime.png)

## 一、Introduce   

This is a simple UGUI style console。In Mobile and Editor can quickly and easily view runtime Log information, and support asynchronous locally stored, the Log file without changing the game running efficiency and did not produce the gc.    
In addition, also provide editable CMD, convenient Runtime debugging    
Suggested that the source code into a DLL, can support the Unity type of Log "jump" immediate.>   
Please Email: weizhuquan@vip.qq.com for help.   

## 二、Feature   

### 2.0、Open   

The default startup shortcut key is ~ key. and mobile should three fingers touch on the screen above 3/4 from the top.    
    
### 2.1、Log    

	using TinyTeam.Debuger;   
	and then：TTDebuger.Log("chiuan");    
	Of course support custom message types: TTDebuger.Log("chiuan","Net");    
	
### 2.2、CMD    
	The default can enter "?" for the registration of all operational instructions    
	for example FPS：fps，you can input arg like> 0、1、2 Using the blank space key separate parameters   
	0:close the fps   
	1:only fps    
	2:fps and memory total show   
 
	Accordingly, want to cancel the registration can be used：TTDebuger.UnRegisterCommand("chiuan");    

	Convenient CMD, such as: "show" only the different types of Log   
	>show net   
	Here convenient to specify the view types of custom Log "net" message   
	NOTE:must be in the Debug message when you pass the custom types:"net"	    

### 2.3、View Log history   

	Provide View the old log file,you can open a log file quickly here:   
	Note:all log file under your project folder/Log   

### 2.4、Debug Buttons

	Provide buttons for quick execute cmd
	1、AddButton : TTDebuger.RegisterButton(string btnName, string cmd)
	2、RemoveButton : TTDebuger.UnRegisterButton(string btnName)
	3、the buttons will show like this

![image](https://github.com/leviyuan/TTConsole/blob/master/remote1.jpg)

	4、click the button, just like type the "cmd"

### 2.5、Remote Console

	Remote console make easy to type cmd to mobiles
	1、open remote, use cmd "remote [port]", or api "TTDebuger.ProcessCmd('remote')"
	2、open remote console application，"TTConsole_Remote_Unity.exe"
	3、type cmd on remote console just like on the phone
	Note:Socket does not close well on pc

![image](https://github.com/chiuan/TTConsole/blob/master/remote1.jpg)
![image](https://github.com/chiuan/TTConsole/blob/master/remote2.jpg)
