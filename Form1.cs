using System.Text.Json;

namespace JsonValApp;

public partial class JsonValApp : Form
{
    private TextBox inputTextBox;
    private TextBox outputTextBox;
    private Button validateButton;
    private Button beautifyButton;

    public JsonValApp()
    {
        InitializeComponent();
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        // Input TextBox
        inputTextBox = new TextBox
        {
            Multiline = true,
            Width = this.ClientSize.Width - 40, // Adjust width to max available with padding
            Height = 130, // Adjust height with padding
            Top = 60, // Adjusted to make space for buttons and padding
            Left = 20, // Add padding
            ScrollBars = ScrollBars.Both, // Add scrollbars to input textbox
            BorderStyle = BorderStyle.FixedSingle // Add light border
        };
        inputTextBox.Name = "inputTextBox";
        Controls.Add(inputTextBox);

        // Input Label
        var inputLabel = new Label
        {
            Text = "Input:",
            Top = inputTextBox.Top - 20, // Position above the input textbox
            Left = inputTextBox.Left, // Align with the input textbox
            AutoSize = true
        };
        Controls.Add(inputLabel);

        // Output TextBox
        outputTextBox = new TextBox
        {
            Multiline = true,
            Width = this.ClientSize.Width, // Adjust width to max available with padding
            Height = this.ClientSize.Height - 230, // Maximize height to fill remaining space with padding
            Top = 220, // Positioned below input textbox with padding
            Left = 10,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both, // Add scrollbars to output textbox
            BorderStyle = BorderStyle.FixedSingle // Add light border
        };
        outputTextBox.Name = "outputTextBox";
        Controls.Add(outputTextBox);

        // Output Label
        var outputLabel = new Label
        {
            Text = "Output:",
            Top = outputTextBox.Top - 20, // Position above the output textbox
            Left = outputTextBox.Left, // Align with the output textbox
            AutoSize = true
        };
        Controls.Add(outputLabel);

        // Validate Button
        validateButton = new Button
        {
            Text = "Validate JSON",
            Top = 10, // Move to top
            Left = 10,
            FlatStyle = FlatStyle.Flat, // Enhance button UI
            BackColor = Color.LightBlue,
            Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
        };
        validateButton.Click += ValidateButton_Click;
        Controls.Add(validateButton);

        // Beautify Button
        beautifyButton = new Button
        {
            Text = "Beautify JSON",
            Top = 10, // Move to top
            Left = 120,
            FlatStyle = FlatStyle.Flat, // Enhance button UI
            BackColor = Color.LightGreen,
            Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
        };
        beautifyButton.Click += BeautifyButton_Click;
        Controls.Add(beautifyButton);

        // Clear Button
        var clearButton = new Button
        {
            Text = "Clear",
            Top = 10, // Align with other buttons
            Left = 230, // Position next to Beautify Button
            FlatStyle = FlatStyle.Flat, // Enhance button UI
            BackColor = Color.LightCoral,
            Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
        };
        clearButton.Click += ClearButton_Click;
        Controls.Add(clearButton);
    }

    private void ValidateButton_Click(object sender, EventArgs e)
    {
        ValidateAndParseJson(inputTextBox.Text);
    }

    private void BeautifyButton_Click(object sender, EventArgs e)
    {
        ValidateAndParseJson(inputTextBox.Text);
    }

    private void ValidateAndParseJson(string jsonString)
    {
        try
        {
            // Log the input string for debugging
            Console.WriteLine("Input JSON String: " + jsonString);

            // Replace escaped quotes with actual quotes
            jsonString = jsonString.Replace("\\\"", "\"");

            // Sanitize the input by removing invalid characters and trimming whitespace
            jsonString = jsonString.Replace("\r", "").Replace("\n", "").Trim();

            // Ensure the input is a valid JSON object or array
            if (!(jsonString.StartsWith("{") && jsonString.EndsWith("}")) &&
                !(jsonString.StartsWith("[") && jsonString.EndsWith("]")))
            {
                throw new JsonException("Input is not a valid JSON object or array.");
            }

            // Attempt to parse the JSON
            using (var document = JsonDocument.Parse(jsonString))
            {
                var jsonElement = document.RootElement;

                // Format the JSON string
                var formattedJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });

                // Override the input textbox value with the formatted JSON
                inputTextBox.Text = formattedJson;

                // Display success message in the output textbox
                outputTextBox.Text = "Valid JSON formatted successfully.";
            }
        }
        catch (JsonException ex)
        {
            // Log the error for debugging
            Console.WriteLine("JSON Exception: " + ex.Message);

            // Handle invalid or incomplete JSON
            outputTextBox.Text = $"Invalid or Incomplete JSON: {ex.Message}";
        }
        catch (Exception ex)
        {
            // Log unexpected errors for debugging
            Console.WriteLine("Unexpected Exception: " + ex.Message);

            // Handle unexpected errors
            outputTextBox.Text = $"An unexpected error occurred: {ex.Message}";
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        inputTextBox.Clear();
        outputTextBox.Clear();
    }
}
