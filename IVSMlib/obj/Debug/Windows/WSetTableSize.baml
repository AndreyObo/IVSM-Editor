   M S B A M L   `   `   `  ????B  >IVSMlib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:IVSMlib.WindowsIVSMlib.Windows  5   	   #    IVSMlib.Windows.WSetTableSize   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:IVSMlib.Windows     5   	   -   5          ? WindowStyle$  
ToolWindow=?5   	    ? Title$! Размер таблицы??5	   	   $??159.687??6    $??223.438??61    ? 
ResizeMode$ NoResize=?6A   .??5
      ? 
??????? ? ColumnDefinitions 5   
   ?? 5      $
??0.8* ?6   ?? 5       ? RowDefinitions 5   
   ?? 5      ?? 5      ?? 5      ?? 5      .+?5   
   ?? $??Center=?6   $??Center=?6/   $??0??6L   $??0??6\   Ширина6q   ?? 5   
   -   5         ??Name$ WidthTB??5      $	??100??6#   $??Center=?6/   $??Center=?6J   $??1??6g   $??0??6w   6?   ?? 5   
   $??Center=?6   $??Center=?6/   $??0??6L   $??1??6\   Высота6q   ?? 5   
   -   5        $??HeightTB??5      $	??100??6"   $??Center=?6.   $??Center=?6I   $??1??6f   $??1??6v   6?   ?? 5   
   -   5         ??Name$ 
CellSizeCB??5      $??5,0,0,0q?6'   $??Center=?68   $??2??6S   $??2??6`   .??6t   ,*Выровнять размер ячеек6?   ?? 5   
   $??5,5,5,5q?6   $??Right=?6&    ?? Orientation$ 
Horizontal=?6B   $??3??6[   $??2??6h   .+?5      ?? -   5        $??0,0,5,0q?5   '   .??68   Принять6A   ?? 5      -   5        .??5   (   Отменить62   5       5!      5"      