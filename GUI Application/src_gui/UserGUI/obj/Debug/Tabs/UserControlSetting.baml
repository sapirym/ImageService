   M S B A M L   `   `   `  ����B  >UserGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:UserGUI.TabsUserGUI.Tabs  5      %    UserGUI.Tabs.UserControlSetting   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:UserGUI.Tabs     5      .��5	      � 
�������  � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��    HeaderStyle#��[  6   
Settings6C   �� 5   
   $��SourceName:��6     	BoldLabel#��[ 6&   $��	0,70,0,67q�6I   �� 5   
   $��0��6=   $��105,77,0,83q�6J   ��6   �� *
SourceName+ �� Mode$ OneWay=�6J   �� 5   
   $��	Log Name:��6   #��[ 6$   $��	0,95,0,45q�6G   �� 5   
   $��89,103,0,60q�6:   ��6   �� *	LogName+$ OneWay=�6:   �� 5    
   $��
OutputDir:��6   #��[ 6%   $��	0,45,0,93q�6H   �� 5!   
   $��87,53,0,111q�6<   ��6   �� *	OutputDir+$ OneWay=�6<   �� 5#   
   $��ThumbnailSize:��6   #��[ 6*   $��
0,120,0,20q�6M   �� 5$   
   $��118,127,0,33q�6@   ��6   �� *ThumbnailSize+$ OneWay=�6@   �� 5%   
   -   5         ��Name$ button��5%      $��Remove��6!   $��Stretch=�62   $��	0,160,0,0q�5'      $	��Top=�6$   $��20��6<   ��5&      �� *removeCommand+5'   <   �� 5(   
   -   5        $��listBox��5(      ��.  �� SelectionMode$ Single=�5*   7   $��Stretch=�5+      
����İ�$
��Auto=�5,      $��Stretch=�6B   $��5��5-      $��Stretch=�6   $��0,1,0,0q�6<   $��6��6M   ��5(   !   �� *	Handler+ �� SelectedItem 5)      �� *
ChooseItem+$ TwoWay=�5-   M   50      51      