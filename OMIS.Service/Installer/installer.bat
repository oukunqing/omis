@echo ��ǰ�̷���
E:
@echo �л�Ŀ¼
cd E:\Projects\ZyrhCms\OMIS\OMIS.Service\bin\Debug

installutil /u OMIS.Service.exe
installutil OMIS.Service.exe

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