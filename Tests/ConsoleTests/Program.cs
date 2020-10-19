using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Linq.Expressions;

using System.Reflection;
namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly asm = Assembly.GetEntryAssembly();

            Type type = asm.GetType("ConsoleTests.Program");
            Type type2 = asm.GetTypes().First(t => t.Name == "Program");

            var str = "Hello World!";

            var type3 = GetObjectType(str);

            var type_string = typeof(string);

            var test_lib_file = new FileInfo("TestLib.dll");
            var test_lib_assembly = Assembly.LoadFile(test_lib_file.FullName);

            var printer_type = test_lib_assembly.GetType("TestLib.Printer");

            //ConstructorInfo
            //MethodInfo
            //ParameterInfo
            //PropertyInfo
            //EventInfo
            //FieldInfo

            foreach (var method in printer_type.GetMethods())
            {
                var return_type = method.ReturnType;
                var parameters = method.GetParameters();

                Console.WriteLine("{0} {1} ({2})",
                    return_type.Name,
                    method.Name,
                    string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}")));
            }

            object printer = Activator.CreateInstance(printer_type, ">>>");

            var printer_constructor = printer_type.GetConstructor(new[] { typeof(string) });

            var printer2 = printer_constructor.Invoke(new object[] { "<<<" });

            var print_method_info = printer_type.GetMethod("Print");

            print_method_info.Invoke(printer, new object[] { "Hello World!" });

            var prefix_field_info = printer_type.GetField("_Prefix", BindingFlags.NonPublic | BindingFlags.Instance);

            object prefix_value_object = prefix_field_info.GetValue(printer);
            var prefix_value_string = (string)prefix_field_info.GetValue(printer);  // раннее связывание

            prefix_field_info.SetValue(printer, "123");

            //var app_domain = AppDomain.CurrentDomain;
            //var test_domain = AppDomain.CreateDomain("TestDomain");
            ////test_domain.ExecuteAssemblyByName();
            //AppDomain.Unload(test_domain);

            //var admin_process_info = new ProcessStartInfo(Assembly.GetEntryAssembly().Location, "/RegistryWrite")
            //{

            //};

            //Process process = Process.Start(admin_process_info);

            //dynamic dynamic_printer = printer;
            //dynamic_printer.Print("12312312312312321");

            Action<string> print_lambda = str => Console.WriteLine(str);

            Expression<Action<string>> print_expression = str => Console.WriteLine(str);

            Action<string> compiled_expression = print_expression.Compile();

            var str_parameter = Expression.Parameter(typeof(string), "str");

            var invoke_node = Expression.Call(
                null, 
                typeof(Console).GetMethod("WriteLine", new[] { typeof(string)}), 
                str_parameter);

            var result_expression = Expression.Lambda<Action<string>>(invoke_node, str_parameter);

            Action<string> compiled_expression2 = print_expression.Compile();

            compiled_expression("QWE");
            compiled_expression2("===");

            Console.ReadLine();
        }

        private static Type GetObjectType(object obj)
        {
            return obj.GetType();
        }
    }
}
