installutil /u OMIS.Service.exe
installutil OMIS.Service.exe

rem %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u OMIS.Service.exe
rem %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe OMIS.Service.exe
@echo ����װ��ϣ����÷���...

@echo ���÷���Ϊ�Զ�����
rem auto-�Զ� demand-�ֶ� disabled-����
sc config OMISService start= auto

@echo ����������������潻��
rem ������������ɾ��type= interact����
sc config OMISService type= interact type= own

@echo ��������...
net start OMISService

@pause