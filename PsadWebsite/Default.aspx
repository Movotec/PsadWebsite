<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PsadWebsite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
<h1>Data
        </h1>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="MeasureGuid" DataSourceID="SqlDataSource1" CssClass="data-grid" EmptyDataText="There are no data records to display.">
    <Columns>
        <asp:BoundField DataField="RecID" HeaderText="RecID" SortExpression="RecID" />
        <asp:BoundField DataField="PsadGuid" HeaderText="PsadGuid" SortExpression="PsadGuid" />
        <asp:BoundField DataField="PatientGuid" HeaderText="PatientGuid" SortExpression="PatientGuid" />
        <asp:BoundField DataField="OperatorGuid" HeaderText="OperatorGuid" SortExpression="OperatorGuid" />
        <asp:BoundField DataField="MeasureMode" HeaderText="MeasureMode" SortExpression="MeasureMode" />
        <asp:BoundField DataField="Limb" HeaderText="Limb" SortExpression="Limb" />
        <asp:BoundField DataField="Orientation" HeaderText="Orientation" SortExpression="Orientation" />
        <asp:BoundField DataField="MeasureTime" HeaderText="MeasureTime" SortExpression="MeasureTime" />
        <asp:BoundField DataField="Comport" HeaderText="Comport" SortExpression="Comport" />
        <asp:BoundField DataField="FibulaLength" HeaderText="FibulaLength" SortExpression="FibulaLength" />
        <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
        <asp:BoundField DataField="FirmWare1" HeaderText="FirmWare1" SortExpression="FirmWare1" />
        <asp:BoundField DataField="FirmWare2" HeaderText="FirmWare2" SortExpression="FirmWare2" />
        <asp:BoundField DataField="FirmWare3" HeaderText="FirmWare3" SortExpression="FirmWare3" />
        <asp:BoundField DataField="MeasureDateTime" HeaderText="MeasureDateTime" SortExpression="MeasureDateTime" />
        <asp:BoundField DataField="MeasureGuid" HeaderText="MeasureGuid" ReadOnly="True" SortExpression="MeasureGuid" />
        <asp:BoundField DataField="MasterLength1" HeaderText="MasterLength1" SortExpression="MasterLength1" />
        <asp:BoundField DataField="MasterLength2" HeaderText="MasterLength2" SortExpression="MasterLength2" />
        <asp:BoundField DataField="StartAngle" HeaderText="StartAngle" SortExpression="StartAngle" />
        <asp:BoundField DataField="MinAngle" HeaderText="MinAngle" SortExpression="MinAngle" />
        <asp:BoundField DataField="MaxAngle" HeaderText="MaxAngle" SortExpression="MaxAngle" />
        <asp:BoundField DataField="Rom" HeaderText="Rom" SortExpression="Rom" />
        <asp:BoundField DataField="MaxAngularVelocity" HeaderText="MaxAngularVelocity" SortExpression="MaxAngularVelocity" />
        <asp:BoundField DataField="MinAngularVelocity" HeaderText="MinAngularVelocity" SortExpression="MinAngularVelocity" />
        <asp:BoundField DataField="MaxAcceleration" HeaderText="MaxAcceleration" SortExpression="MaxAcceleration" />
        <asp:BoundField DataField="MinForce" HeaderText="MinForce" SortExpression="MinForce" />
        <asp:BoundField DataField="MaxForce" HeaderText="MaxForce" SortExpression="MaxForce" />
        <asp:BoundField DataField="MinXForce" HeaderText="MinXForce" SortExpression="MinXForce" />
        <asp:BoundField DataField="MaxXForce" HeaderText="MaxXForce" SortExpression="MaxXForce" />
        <asp:BoundField DataField="MinYForce" HeaderText="MinYForce" SortExpression="MinYForce" />
        <asp:BoundField DataField="MaxYForce" HeaderText="MaxYForce" SortExpression="MaxYForce" />
        <asp:BoundField DataField="Stiffness1" HeaderText="Stiffness1" SortExpression="Stiffness1" />
        <asp:BoundField DataField="Stiffness2" HeaderText="Stiffness2" SortExpression="Stiffness2" />
        <asp:BoundField DataField="Stiffness3" HeaderText="Stiffness3" SortExpression="Stiffness3" />
        <asp:BoundField DataField="Stiffness4" HeaderText="Stiffness4" SortExpression="Stiffness4" />
        <asp:BoundField DataField="Stiffness5" HeaderText="Stiffness5" SortExpression="Stiffness5" />
        <asp:BoundField DataField="EMG1Activity" HeaderText="EMG1Activity" SortExpression="EMG1Activity" />
        <asp:BoundField DataField="EMG2Activity" HeaderText="EMG2Activity" SortExpression="EMG2Activity" />
        <asp:CheckBoxField DataField="Batt0" HeaderText="Batt0" SortExpression="Batt0" />
        <asp:CheckBoxField DataField="Batt1" HeaderText="Batt1" SortExpression="Batt1" />
        <asp:CheckBoxField DataField="Batt2" HeaderText="Batt2" SortExpression="Batt2" />
        <asp:CheckBoxField DataField="Led0" HeaderText="Led0" SortExpression="Led0" />
        <asp:CheckBoxField DataField="Led1" HeaderText="Led1" SortExpression="Led1" />
        <asp:CheckBoxField DataField="Led2" HeaderText="Led2" SortExpression="Led2" />
        <asp:CheckBoxField DataField="Led3" HeaderText="Led3" SortExpression="Led3" />
        <asp:CheckBoxField DataField="Led4" HeaderText="Led4" SortExpression="Led4" />
        <asp:BoundField DataField="Acc1XOffset" HeaderText="Acc1XOffset" SortExpression="Acc1XOffset" />
        <asp:BoundField DataField="Acc1XGain" HeaderText="Acc1XGain" SortExpression="Acc1XGain" />
        <asp:BoundField DataField="Acc1YOffset" HeaderText="Acc1YOffset" SortExpression="Acc1YOffset" />
        <asp:BoundField DataField="Acc1YGain" HeaderText="Acc1YGain" SortExpression="Acc1YGain" />
        <asp:BoundField DataField="Acc1ZOffset" HeaderText="Acc1ZOffset" SortExpression="Acc1ZOffset" />
        <asp:BoundField DataField="Acc1ZGain" HeaderText="Acc1ZGain" SortExpression="Acc1ZGain" />
        <asp:BoundField DataField="Acc2XOffset" HeaderText="Acc2XOffset" SortExpression="Acc2XOffset" />
        <asp:BoundField DataField="Acc2XGain" HeaderText="Acc2XGain" SortExpression="Acc2XGain" />
        <asp:BoundField DataField="Acc2YOffset" HeaderText="Acc2YOffset" SortExpression="Acc2YOffset" />
        <asp:BoundField DataField="Acc2YGain" HeaderText="Acc2YGain" SortExpression="Acc2YGain" />
        <asp:BoundField DataField="Acc2ZOffset" HeaderText="Acc2ZOffset" SortExpression="Acc2ZOffset" />
        <asp:BoundField DataField="Acc2ZGain" HeaderText="Acc2ZGain" SortExpression="Acc2ZGain" />
        <asp:BoundField DataField="GyroXOffset" HeaderText="GyroXOffset" SortExpression="GyroXOffset" />
        <asp:BoundField DataField="GyroXGain" HeaderText="GyroXGain" SortExpression="GyroXGain" />
        <asp:BoundField DataField="GyroYOffset" HeaderText="GyroYOffset" SortExpression="GyroYOffset" />
        <asp:BoundField DataField="GyroYGain" HeaderText="GyroYGain" SortExpression="GyroYGain" />
        <asp:BoundField DataField="GyroZOffset" HeaderText="GyroZOffset" SortExpression="GyroZOffset" />
        <asp:BoundField DataField="GyroZGain" HeaderText="GyroZGain" SortExpression="GyroZGain" />
        <asp:BoundField DataField="Emg1Offset" HeaderText="Emg1Offset" SortExpression="Emg1Offset" />
        <asp:BoundField DataField="Emg1Gain" HeaderText="Emg1Gain" SortExpression="Emg1Gain" />
        <asp:BoundField DataField="Emg2Offset" HeaderText="Emg2Offset" SortExpression="Emg2Offset" />
        <asp:BoundField DataField="Emg2Gain" HeaderText="Emg2Gain" SortExpression="Emg2Gain" />
        <asp:BoundField DataField="Straingauge1Offset" HeaderText="Straingauge1Offset" SortExpression="Straingauge1Offset" />
        <asp:BoundField DataField="Straingauge1Gain" HeaderText="Straingauge1Gain" SortExpression="Straingauge1Gain" />
        <asp:BoundField DataField="Straingauge2Offset" HeaderText="Straingauge2Offset" SortExpression="Straingauge2Offset" />
        <asp:BoundField DataField="Straingauge2Gain" HeaderText="Straingauge2Gain" SortExpression="Straingauge2Gain" />
        <asp:BoundField DataField="Straingauge3Offset" HeaderText="Straingauge3Offset" SortExpression="Straingauge3Offset" />
        <asp:BoundField DataField="Straingauge3Gain" HeaderText="Straingauge3Gain" SortExpression="Straingauge3Gain" />
        <asp:BoundField DataField="Straingauge4Offset" HeaderText="Straingauge4Offset" SortExpression="Straingauge4Offset" />
        <asp:BoundField DataField="Straingauge4Gain" HeaderText="Straingauge4Gain" SortExpression="Straingauge4Gain" />
        <asp:BoundField DataField="PsadWeight" HeaderText="PsadWeight" SortExpression="PsadWeight" />
        <asp:BoundField DataField="PsadAcc12" HeaderText="PsadAcc12" SortExpression="PsadAcc12" />
        <asp:BoundField DataField="PsadAcc1h" HeaderText="PsadAcc1h" SortExpression="PsadAcc1h" />
        <asp:BoundField DataField="PsadAcc2h" HeaderText="PsadAcc2h" SortExpression="PsadAcc2h" />
        <asp:BoundField DataField="Psadh" HeaderText="Psadh" SortExpression="Psadh" />
        <asp:BoundField DataField="AccGRange" HeaderText="AccGRange" SortExpression="AccGRange" />
        <asp:BoundField DataField="GyroDegreePrSec" HeaderText="GyroDegreePrSec" SortExpression="GyroDegreePrSec" />
        <asp:BoundField DataField="SPS" HeaderText="SPS" SortExpression="SPS" />
        <asp:BoundField DataField="Emg1Pad" HeaderText="Emg1Pad" SortExpression="Emg1Pad" />
        <asp:BoundField DataField="Emg2Pad" HeaderText="Emg2Pad" SortExpression="Emg2Pad" />
        <asp:BoundField DataField="BatteryLow" HeaderText="BatteryLow" SortExpression="BatteryLow" />
        <asp:BoundField DataField="BatteryHigh" HeaderText="BatteryHigh" SortExpression="BatteryHigh" />
        <asp:BoundField DataField="LastCalibrationDateAndTime" HeaderText="LastCalibrationDateAndTime" SortExpression="LastCalibrationDateAndTime" />
    </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PsadData %>" DeleteCommand="DELETE FROM [Measurements] WHERE [MeasureGuid] = @MeasureGuid" InsertCommand="INSERT INTO [Measurements] ([RecID], [PsadGuid], [PatientGuid], [OperatorGuid], [MeasureMode], [Limb], [Orientation], [MeasureTime], [Comport], [FibulaLength], [Comment], [FirmWare1], [FirmWare2], [FirmWare3], [MeasureDateTime], [MeasureGuid], [MasterLength1], [MasterLength2], [StartAngle], [MinAngle], [MaxAngle], [Rom], [MaxAngularVelocity], [MinAngularVelocity], [MaxAcceleration], [MinForce], [MaxForce], [MinXForce], [MaxXForce], [MinYForce], [MaxYForce], [Stiffness1], [Stiffness2], [Stiffness3], [Stiffness4], [Stiffness5], [EMG1Activity], [EMG2Activity], [Batt0], [Batt1], [Batt2], [Led0], [Led1], [Led2], [Led3], [Led4], [Acc1XOffset], [Acc1XGain], [Acc1YOffset], [Acc1YGain], [Acc1ZOffset], [Acc1ZGain], [Acc2XOffset], [Acc2XGain], [Acc2YOffset], [Acc2YGain], [Acc2ZOffset], [Acc2ZGain], [GyroXOffset], [GyroXGain], [GyroYOffset], [GyroYGain], [GyroZOffset], [GyroZGain], [Emg1Offset], [Emg1Gain], [Emg2Offset], [Emg2Gain], [Straingauge1Offset], [Straingauge1Gain], [Straingauge2Offset], [Straingauge2Gain], [Straingauge3Offset], [Straingauge3Gain], [Straingauge4Offset], [Straingauge4Gain], [PsadWeight], [PsadAcc12], [PsadAcc1h], [PsadAcc2h], [Psadh], [AccGRange], [GyroDegreePrSec], [SPS], [Emg1Pad], [Emg2Pad], [BatteryLow], [BatteryHigh], [LastCalibrationDateAndTime]) VALUES (@RecID, @PsadGuid, @PatientGuid, @OperatorGuid, @MeasureMode, @Limb, @Orientation, @MeasureTime, @Comport, @FibulaLength, @Comment, @FirmWare1, @FirmWare2, @FirmWare3, @MeasureDateTime, @MeasureGuid, @MasterLength1, @MasterLength2, @StartAngle, @MinAngle, @MaxAngle, @Rom, @MaxAngularVelocity, @MinAngularVelocity, @MaxAcceleration, @MinForce, @MaxForce, @MinXForce, @MaxXForce, @MinYForce, @MaxYForce, @Stiffness1, @Stiffness2, @Stiffness3, @Stiffness4, @Stiffness5, @EMG1Activity, @EMG2Activity, @Batt0, @Batt1, @Batt2, @Led0, @Led1, @Led2, @Led3, @Led4, @Acc1XOffset, @Acc1XGain, @Acc1YOffset, @Acc1YGain, @Acc1ZOffset, @Acc1ZGain, @Acc2XOffset, @Acc2XGain, @Acc2YOffset, @Acc2YGain, @Acc2ZOffset, @Acc2ZGain, @GyroXOffset, @GyroXGain, @GyroYOffset, @GyroYGain, @GyroZOffset, @GyroZGain, @Emg1Offset, @Emg1Gain, @Emg2Offset, @Emg2Gain, @Straingauge1Offset, @Straingauge1Gain, @Straingauge2Offset, @Straingauge2Gain, @Straingauge3Offset, @Straingauge3Gain, @Straingauge4Offset, @Straingauge4Gain, @PsadWeight, @PsadAcc12, @PsadAcc1h, @PsadAcc2h, @Psadh, @AccGRange, @GyroDegreePrSec, @SPS, @Emg1Pad, @Emg2Pad, @BatteryLow, @BatteryHigh, @LastCalibrationDateAndTime)" ProviderName="<%$ ConnectionStrings:PsadData.ProviderName %>" SelectCommand="SELECT [RecID], [PsadGuid], [PatientGuid], [OperatorGuid], [MeasureMode], [Limb], [Orientation], [MeasureTime], [Comport], [FibulaLength], [Comment], [FirmWare1], [FirmWare2], [FirmWare3], [MeasureDateTime], [MeasureGuid], [MasterLength1], [MasterLength2], [StartAngle], [MinAngle], [MaxAngle], [Rom], [MaxAngularVelocity], [MinAngularVelocity], [MaxAcceleration], [MinForce], [MaxForce], [MinXForce], [MaxXForce], [MinYForce], [MaxYForce], [Stiffness1], [Stiffness2], [Stiffness3], [Stiffness4], [Stiffness5], [EMG1Activity], [EMG2Activity], [Batt0], [Batt1], [Batt2], [Led0], [Led1], [Led2], [Led3], [Led4], [Acc1XOffset], [Acc1XGain], [Acc1YOffset], [Acc1YGain], [Acc1ZOffset], [Acc1ZGain], [Acc2XOffset], [Acc2XGain], [Acc2YOffset], [Acc2YGain], [Acc2ZOffset], [Acc2ZGain], [GyroXOffset], [GyroXGain], [GyroYOffset], [GyroYGain], [GyroZOffset], [GyroZGain], [Emg1Offset], [Emg1Gain], [Emg2Offset], [Emg2Gain], [Straingauge1Offset], [Straingauge1Gain], [Straingauge2Offset], [Straingauge2Gain], [Straingauge3Offset], [Straingauge3Gain], [Straingauge4Offset], [Straingauge4Gain], [PsadWeight], [PsadAcc12], [PsadAcc1h], [PsadAcc2h], [Psadh], [AccGRange], [GyroDegreePrSec], [SPS], [Emg1Pad], [Emg2Pad], [BatteryLow], [BatteryHigh], [LastCalibrationDateAndTime] FROM [Measurements]" UpdateCommand="UPDATE [Measurements] SET [RecID] = @RecID, [PsadGuid] = @PsadGuid, [PatientGuid] = @PatientGuid, [OperatorGuid] = @OperatorGuid, [MeasureMode] = @MeasureMode, [Limb] = @Limb, [Orientation] = @Orientation, [MeasureTime] = @MeasureTime, [Comport] = @Comport, [FibulaLength] = @FibulaLength, [Comment] = @Comment, [FirmWare1] = @FirmWare1, [FirmWare2] = @FirmWare2, [FirmWare3] = @FirmWare3, [MeasureDateTime] = @MeasureDateTime, [MasterLength1] = @MasterLength1, [MasterLength2] = @MasterLength2, [StartAngle] = @StartAngle, [MinAngle] = @MinAngle, [MaxAngle] = @MaxAngle, [Rom] = @Rom, [MaxAngularVelocity] = @MaxAngularVelocity, [MinAngularVelocity] = @MinAngularVelocity, [MaxAcceleration] = @MaxAcceleration, [MinForce] = @MinForce, [MaxForce] = @MaxForce, [MinXForce] = @MinXForce, [MaxXForce] = @MaxXForce, [MinYForce] = @MinYForce, [MaxYForce] = @MaxYForce, [Stiffness1] = @Stiffness1, [Stiffness2] = @Stiffness2, [Stiffness3] = @Stiffness3, [Stiffness4] = @Stiffness4, [Stiffness5] = @Stiffness5, [EMG1Activity] = @EMG1Activity, [EMG2Activity] = @EMG2Activity, [Batt0] = @Batt0, [Batt1] = @Batt1, [Batt2] = @Batt2, [Led0] = @Led0, [Led1] = @Led1, [Led2] = @Led2, [Led3] = @Led3, [Led4] = @Led4, [Acc1XOffset] = @Acc1XOffset, [Acc1XGain] = @Acc1XGain, [Acc1YOffset] = @Acc1YOffset, [Acc1YGain] = @Acc1YGain, [Acc1ZOffset] = @Acc1ZOffset, [Acc1ZGain] = @Acc1ZGain, [Acc2XOffset] = @Acc2XOffset, [Acc2XGain] = @Acc2XGain, [Acc2YOffset] = @Acc2YOffset, [Acc2YGain] = @Acc2YGain, [Acc2ZOffset] = @Acc2ZOffset, [Acc2ZGain] = @Acc2ZGain, [GyroXOffset] = @GyroXOffset, [GyroXGain] = @GyroXGain, [GyroYOffset] = @GyroYOffset, [GyroYGain] = @GyroYGain, [GyroZOffset] = @GyroZOffset, [GyroZGain] = @GyroZGain, [Emg1Offset] = @Emg1Offset, [Emg1Gain] = @Emg1Gain, [Emg2Offset] = @Emg2Offset, [Emg2Gain] = @Emg2Gain, [Straingauge1Offset] = @Straingauge1Offset, [Straingauge1Gain] = @Straingauge1Gain, [Straingauge2Offset] = @Straingauge2Offset, [Straingauge2Gain] = @Straingauge2Gain, [Straingauge3Offset] = @Straingauge3Offset, [Straingauge3Gain] = @Straingauge3Gain, [Straingauge4Offset] = @Straingauge4Offset, [Straingauge4Gain] = @Straingauge4Gain, [PsadWeight] = @PsadWeight, [PsadAcc12] = @PsadAcc12, [PsadAcc1h] = @PsadAcc1h, [PsadAcc2h] = @PsadAcc2h, [Psadh] = @Psadh, [AccGRange] = @AccGRange, [GyroDegreePrSec] = @GyroDegreePrSec, [SPS] = @SPS, [Emg1Pad] = @Emg1Pad, [Emg2Pad] = @Emg2Pad, [BatteryLow] = @BatteryLow, [BatteryHigh] = @BatteryHigh, [LastCalibrationDateAndTime] = @LastCalibrationDateAndTime WHERE [MeasureGuid] = @MeasureGuid">

    </asp:SqlDataSource>

 <%--       <asp:Repeater ID="RepeaterTest" runat="server" >
            <ItemTemplate><p><%# Eval("id") %> </p></ItemTemplate>
        </asp:Repeater>--%>

        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>

        <asp:Button ID="ButtonClean" runat="server" Text="PsadData Import to sql" OnClick="ButtonClean_Click" />

        <asp:Button ID="ButtonReset" runat="server" Text="Reset: CleanseDb and move files back to data folder" OnClick="ButtonReset_Click" />

        <asp:Button ID="ButtonEntityFramework" runat="server" Text="Create Measurement with fake data via Entity framework" OnClick="ButtonEntityFramework_Click" />
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
