using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mandelbrot
{
    public partial class HelpForm : Form
    {
        Dictionary<string, string> helpText;

        public HelpForm()
        {
            InitializeComponent();
            helpTreeView.ExpandAll();
            helpText = new Dictionary<string, string>();
            helpText.Add("Mandelbrot Generator", "Welkom bij de helpfunctie van de Mandelbrot Generator waar u informatie en uitleg vindt over alle mogelijkheden van dit programma.");
            helpText.Add("Start menu", "In het start menu vindt u de mogelijkheid om een nieuwe afbeelding te genereren, de huidige afbeelding op te slaan en het programma af te sluiten. Voor meer informatie over de individuele mogelijkheden klikt u op het bijbehorend onderdeel.");
            helpText.Add("Nieuw", "Door op 'Nieuw' te klikken wordt de mandelbrot terug gezet naar de oorspronkelijke beginwaarden.");
            helpText.Add("Afbeelding opslaan", "U kunt de huidige afbeelding opslaan als een bitmap, png, jpeg of gif afbeelding. Om verlies van kwaliteit te voorkomen kan de afbeelding echter het beste als bitmap worden opgeslagen.");
            helpText.Add("Afsluiten", "Bij het afsluiten van het programma gaan de huidige instellingen verloren. Het programma begint de volgende keer dat het gestart wordt weer met de standaard waarden.");
            helpText.Add("Invoeropties", "U kunt handmatig de individuele parameters de Mandelbrot Generator aanpassen. Om de wijziging door te voeren klikt u op 'Start' of drukt u op Enter/Return. Voor meer informatie over de individuele invoervelden klikt u op het bijbehorend onderdeel.");
            helpText.Add("Midden X en Y", "In de velden 'Midden X' en 'Midden Y' staan de x en y coordinaten van het middelpunt van de huidige afbeelding vermeld. U kunt deze waarden handmatig aanpassen of door middel van een muisklik op de afbeelding een nieuw middelpunt kiezen.");
            helpText.Add("Schaal", "De schaal bepaalt de mate van vergroting van de afbeelding. Hoe kleiner de schaal, hoe meer op het huidige middelpunt wordt ingezoomd.\r\n\r\n Een schaal kleiner dan E-16 is niet mogelijk, er zal dan de foutmelding 'Limiet schaal overschreden' verschijnen.");
            helpText.Add("Iteraties", "De afbeelding wordt gegenereerd aan de hand van de mandelbrot-formule, welke een aantal keer wordt uitgevoerd. Het maximum aantal herhalingen ofwel iteraties kunt u zelf opgeven. Het aantal iteraties bepaalt de kwaliteit van de afbeelding, wat vooral bij een zeer kleine schaal goed merkbaar is. Bij kleine schaal en weinig iteraties is de kans groot dat er veel zwarte gebieden ontstaan; iteraties verhogen is hiervoor de oplossing.\r\n\r\n Het aantal iteraties is een afweging tussen kwaliteit en snelheid. Hoge iteraties zorgen vaak voor een mooiere afbeelding met meer detail, maar het kost aanzienlijk meer tijd om te genereren.");
            helpText.Add("Kleuren", "U heeft de mogelijkheid om een van de standaard kleurenschema's te kiezen of er zelf de kleuren samen te stellen met behulp van de kleurenkiezer. Vink hiervoor het vakje 'eigen kleur' aan en klik op een van de gekleurde vakken om de kleur aan te passen.");
            helpText.Add("Muis", "De Mandelbrot Generator is vrijwel geheel met de muis te bedienen. Klik op een van de onderdelen om te zien op welke wijze u de afbeelding allemaal kunt aanpassen.");
            helpText.Add("Klikken", "Door middel van een enkele klik van de linkermuisknop op de afbeelding centreert u de mandelbrot op dat punt.");
            helpText.Add("Scrollen", "Door vooruit te scrollen met het muiswiel zoomt u in op het huidige middelpunt van de afbeelding, door achteruit te scrollen kunt u weer uitzoomen.\r\n\r\n Let op: Inzoomen voorbij een schaal kleiner dan E-16 is niet mogelijk. Er zal dan de foutmelding 'Limiet schaal overschreden' verschijnen.");
            helpText.Add("Slepen", "Klik met de linkermuisknop op de afbeelding en houdt deze ingedrukt. Door met de muis te bewegen zal het middelpunt van de afbeelding nu met de richting van de muis mee bewegen.");
            helpRichTextBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 16F, FontStyle.Bold);
            helpRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            helpRichTextBox.SelectedText = "Mandelbrot Generator\r\n\r\n";
            helpRichTextBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 12F, FontStyle.Regular);
            helpRichTextBox.SelectionAlignment = HorizontalAlignment.Left;
            helpRichTextBox.SelectedText = helpText["Mandelbrot Generator"];
        }

        private void showHelpInformation(object o, MouseEventArgs e)
        {
            TreeNode currentNode = helpTreeView.SelectedNode;
            helpRichTextBox.Clear();
            helpRichTextBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 16F, FontStyle.Bold);
            helpRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            helpRichTextBox.SelectedText = currentNode.FullPath + "\r\n\r\n";
            helpRichTextBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 12F, FontStyle.Regular);
            helpRichTextBox.SelectionAlignment = HorizontalAlignment.Left;
            string richtTextBoxContent;
            helpText.TryGetValue(currentNode.Text, out richtTextBoxContent);
            helpRichTextBox.SelectedText = richtTextBoxContent;
        }
    }
}
