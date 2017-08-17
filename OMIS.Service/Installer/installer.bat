@echo 当前盘符：
E:
@echo 切换目录
cd E:\Projects\ZyrhCms\OMIS\OMIS.Service\bin\Debug

installutil /u OMIS.Service.exe
installutil OMIS.Service.exe

@echo 服务安装完毕，设置服务...

@echo 设置服务为自动启动
rem auto-自动 demand-手动 disabled-禁用
sc config OMISService start= auto

@echo 设置允许服务与桌面交互
rem 若不允许交互，删除type= interact即可
sc config OMISService type= interact type= own

@echo 启动服务...
net start OMISService

@pause