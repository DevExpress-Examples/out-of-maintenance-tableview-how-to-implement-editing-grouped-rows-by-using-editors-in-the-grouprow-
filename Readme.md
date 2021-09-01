<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128653949/21.1.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T192300)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [CheckableGroupBehavior.cs](./CS/DevExpress.Example03/CheckableGroupBehavior.cs) (VB: [CheckableGroupBehavior.vb](./VB/DevExpress.Example03/CheckableGroupBehavior.vb))
* [DataHelper.cs](./CS/DevExpress.Example03/DataHelper.cs) (VB: [DataHelper.vb](./VB/DevExpress.Example03/DataHelper.vb))
* [EditorWrapper.cs](./CS/DevExpress.Example03/EditorWrapper.cs) (VB: [EditorWrapper.vb](./VB/DevExpress.Example03/EditorWrapper.vb))
* [Employee.cs](./CS/DevExpress.Example03/Employee.cs) (VB: [Employee.vb](./VB/DevExpress.Example03/Employee.vb))
* [MainWindow.xaml](./CS/DevExpress.Example03/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/DevExpress.Example03/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/DevExpress.Example03/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/DevExpress.Example03/MainWindow.xaml.vb))
<!-- default file list end -->
# TableView - How to implement editing grouped rows by using editors in the GroupRow


<p> </p>
<p>This example illustrates how to display a corresponding editor in the GroupRow based on GridColumn.EditSettings. When this editor loses focus, the entered value is provided into the corresponding GridColumn cells for all rows in this group. <br />This behavior is achieved by implementing the <strong>EditorWrapper</strong> control, which dynamically creates an editor based on the column's settings. It also handles the <strong>PreviewKeyDown</strong> and <strong>LostKeyboardFocus</strong> events for an editor and posts changes to the datasource in the handlers. </p>

<br/>


