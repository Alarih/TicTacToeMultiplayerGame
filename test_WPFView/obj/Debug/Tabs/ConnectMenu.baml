   M S B A M L   `   `   `  ����G  Ctest_WPFView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:test_WPFView.Tabstest_WPFView.Tabs  5      #    test_WPFView.Tabs.ConnectMenu   5   
   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      J FWpfAnimatedGif, Version=1.4.14.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5      +localclr-namespace:test_WPFView.Tabs  5      $y�	Collapsed=�5      .��5      � ��5   
   �� 5        �� ImageSource$  /Images/bg.jpg��6   .+�5   
   �  � RowDefinitions 5      �� 5      $
��170* �6    �� 5      $	��35* �6    �� 5      $	��34* �6    �� 5      $	��32* �6    �� 5      $	��34* �6    �� 5      $	��35* �6    �� 5      $	��34* �6    �� 5      $
��195* �6     � ColumnDefinitions 5      �� 5      $	��48* �6#   �� 5      $	��74* �6#   �� 5       $	��79* �6#   �� 5!      $	��45* �6#   .+�5$      �� -   5         ��Name$ label��5$      
�������
���  ��$��IP��6G   $��1��6T   $��1��6d   $��18.667(�6q   �� 5%      -   5        $ 
label_Copy��5%      
�������
���  ��$��Введите порт��6L   $��1��6c   $��2��6s   $��18.667(�6�   �� 5&      -   5         ��Name$ IPBox��5&      $��2��6%   $��1��65    �� TextWrapping$
 Wrap=�6B   $��192.168.0.1��6V   $��18.667(�6i   6{   �� 5'      -   5        $ portBox��5'      $��2��6'   $��2��67   $
 Wrap=�6D   $��11000��6X   $��18.667(�6e   6w   �� 5(      -   5         ��Name$ 
connectBtn��5(      $��Подключиться��6)   $��1��6@   $��4��6P   $��18.667(�6]   6o   �� 5)      -   5        $ menuBtn��5)      $��Меню��6&   $��1��65   $��6��6E   $��18.667(�6R   6d   5+      5-      5.      