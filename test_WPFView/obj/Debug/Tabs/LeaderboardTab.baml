   M S B A M L   `   `   `  ����G  Ctest_WPFView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:test_WPFView.Tabstest_WPFView.Tabs  5      &     test_WPFView.Tabs.LeaderboardTab   5   
   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:test_WPFView.Tabs     5      J FWpfAnimatedGif, Version=1.4.14.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5      $y�	Collapsed=�5
        � 	Resources  5         ItemCollectionViewSource �� CollectionViewType��%"   &	        �� 5   
   " ��6@   5      .��5      � ��5   
   �� 5       �� ImageSource$ /Images/bg.jpg��6   .+�5   
   �� .+�5      �� $	��250��6   .��5      &   System.Windows.Controls.DataGrid  -   5        $��leaderBoardGrid��5       � DataContext# [  5         AutoGenerateColumns .     CanUserDeleteRows .  $
��Left=�5      $	��Top=�5      "   VerticalScrollBarVisibility$
 Auto=�5         
IsReadOnly . $��18.667(�5    %    J� OpacityMask
 �����
���  ����5      �� ��5    7   Z� 	   Columns	 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 1* 6-      Header$ Имя��68   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   ��  �� Path$
 Name�  5$      $
 1* 6-   $ Победы��68    6H   �� $ WinGamesCounter�  5%      $
 1* 6-   $ 
Ничьи��68    6G   �� $ DrawGamesCounter�  5&      $
 1* 6-   $ Поражения��68    6K   �� $ LoseGamesCounter�5(      5)      �� 5*      -   5        $��menuBtn��5*      $��Меню��6$   $��0,50,0,0q�6I   $	��200��6[   $��52��6g   $��29.333(�6s   5+      5,      5-      