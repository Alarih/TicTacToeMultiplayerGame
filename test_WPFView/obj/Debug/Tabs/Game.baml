   M S B A M L   `   `   `  ����G  Ctest_WPFView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:test_WPFView.Tabstest_WPFView.Tabs  5          test_WPFView.Tabs.Game   5   
   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:test_WPFView.Tabs     5      $y�	Collapsed=�5	      .��5
      � -   5          �Name$  mainGrid��5
       � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   $��42��6*   .+�5   
   �� -   5         ��Name$ 
mainCanvas��5      � 5   
   -   5        $  UIGrid��5      $��1��6    5      �� 5      $��1* �6#   �� 5      $��1* �6#    5      �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    .+�5"      �� -   5         ��Name$ 	turnLabel��5"      $�� ��6'   $��0��62   $��0��6B   $
��Left=�6O   $	��Top=�6j   
�������
���   �6�   �� 5#      -   5        $ scoresLabel��5#      $�� ��6)   $��0��64   $��1��6D   $
��Left=�6Q   $	��Top=�6l   �� 5$      -   5        $ lastScoreLabel��5$      $��0��6,   $��2��6<   $��Игра до��6I   $
��Left=�6[   $	��Top=�6v   $��2��6�   �� 5%      -   5        $ 
crossLabel��5%      $��1��6(   $��2��68   $��Зачеркивание��6E   $
��Left=�6\   $	��Top=�6w   $��2��6�   �� 5&      -   5         ��Name$ disconnectBtn��5&      $��Выйти в меню��6,   $��1��6C   $��0��6S   6`   5'      5(      5)      