   M S B A M L   `   `   `  ����B  >UserGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:UserGUI.ViewModelUserGUI.ViewModel  5      %    UserGUI.Tabs.UserControlLogging   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:UserGUI.ViewModel     5        � 	Resources  5
      $  UserGUI.ViewModel.ConvertColor   ConvertColor��%   &	          5   
   6   5      .��5      � 
�������.+�5   
   �� $��0��6   $��2��6!   $
��Logs��65     HeaderStyle#��[ 6A   6h   �� 5   
   $��0��6   $��3��6$   $��Stretch=�68   .��5      &   System.Windows.Controls.DataGrid  $��10��6      AutoGenerateColumns .     CanUserAddRows .  "   VerticalScrollBarVisibility$
 Auto=�5      ��. $
��Auto��6_   $
��Auto��5      $��0,70,0,0q�6%   ��5   %   �� *Logs+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Type��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 67      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6V   �� 	 �� Path$
	 Type�
   ElementStyle
 5      �� 5       �� 
TargetType" ��6$   .�5   "   ��  �� Property � �� �� Value 6?   �� $
	 Type� �� 	Converter# [  6�   5      5        5      $ Message��6)   $ * 6:    .  6V   �� $	 Message�5      5       5!      5"      