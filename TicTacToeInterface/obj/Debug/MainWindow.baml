   M S B A M L   `   `   `  ����M  ITicTacToeInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:TicTacToeInterfaceTicTacToeInterface  5   	   #    TicTacToeInterface.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:TicTacToeInterface     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��1* �6   6*   �� 5      $��7* �6   6*   .+�5   
   ��  �� TextAlignment$ Center=�6   $��Center=�6+   
����������	#FF012BE8$��
Wide Latin)�6p   $��24(�6�   $
��Bold"�6�   $��Italic$�6�   ��6�   Z� Крестики-нолики6�   � 5   
   $��1��6   $��0q�6    � ColumnDefinitions 5      �� 5      $��1* �6#   60   �� 5      $��1* �6#   60   �� 5      $��1* �6#   60    5      �� 5      $��1* �6    6.   �� 5      $��1* �6    6.   �� 5      $��1* �6    6.   �� 5      $��1* �6    6.   �� 5      $��1* �6    6.   .+�5      �� -   5        $��1��5      $��1��6"   $��5q�62   ���	#FF007ACC
�������.��6|   Начать игру6�   �� 5       -   5        $��2��5       $��1��6"   $��5q�62   ���	#FF007ACC
�������.��6~   Настройки6�   �� 5!      $��3��6   $��1��6"   $��5q�62   ���	#FF007ACC
�������.��6g   %#Статистика игроков6{   5"      5#      5$      