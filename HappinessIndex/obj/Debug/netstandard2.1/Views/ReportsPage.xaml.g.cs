//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("HappinessIndex.Views.ReportsPage.xaml", "Views/ReportsPage.xaml", typeof(global::HappinessIndex.Views.ReportsPage))]

namespace HappinessIndex.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views/ReportsPage.xaml")]
    public partial class ReportsPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.RowDefinition OverallRow;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label FromLabel;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label ToLabel;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label Header;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.SfChart.XForms.SfChart OverallChart;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.SfChart.XForms.SfChart BarChart;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.SfChart.XForms.SfChart Inhibitors;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.XForms.Buttons.SfSegmentedControl selector;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(ReportsPage));
            OverallRow = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.RowDefinition>(this, "OverallRow");
            FromLabel = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "FromLabel");
            ToLabel = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "ToLabel");
            Header = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "Header");
            OverallChart = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.SfChart.XForms.SfChart>(this, "OverallChart");
            BarChart = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.SfChart.XForms.SfChart>(this, "BarChart");
            Inhibitors = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.SfChart.XForms.SfChart>(this, "Inhibitors");
            selector = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.XForms.Buttons.SfSegmentedControl>(this, "selector");
        }
    }
}
