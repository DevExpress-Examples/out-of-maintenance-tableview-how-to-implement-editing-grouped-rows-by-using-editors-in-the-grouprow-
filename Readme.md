# TableView - How to implement editing grouped rows by using editors in the GroupRow


<p> </p>
<p>This example illustrates how to display a corresponding editor in the GroupRow based on GridColumn.EditSettings. When this editor loses focus, the entered value is provided into the corresponding GridColumn cells for all rows in this group. <br />This behavior is achieved by implementing the <strong>EditorWrapper</strong> control, which dynamically creates an editor based on the column's settings. It also handles the <strong>PreviewKeyDown</strong> and <strong>LostKeyboardFocus</strong> events for an editor and posts changes to the datasource in the handlers. </p>

<br/>


