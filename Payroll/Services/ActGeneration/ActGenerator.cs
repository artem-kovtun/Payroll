using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Payroll.Models.Views;
using Payroll.Services.Currency;
using static Payroll.Services.ActGeneration.MoneyToStr;

namespace Payroll.Services.ActGeneration
{
    public class ActGenerator : IActGenerator
    {
        private MoneyToStr _moneyToStr = new MoneyToStr("UAH", "UKR", string.Empty);
        private Money money; 
        private ICurrencyHandler _currencyHandler { get; set; }
        private float UsdExchangeRate { get; set; }

        public ActGenerator(ICurrencyHandler currencyHandler)
        {
            _currencyHandler = currencyHandler;
        }

        public string Generate(ActGenerationViewModel model)
        {
            string filepath;
            money = _moneyToStr.ConvertValueExtended(model.TotalPay);
            UsdExchangeRate = (float)Math.Round(_currencyHandler.GetUsdExchangeByDate(model.WorkCompletionDate).Result.Data.Rate,2);

            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    mainPart.Document = new Document();
                    Body body = new Body();

                    SectionProperties sectionProps = new SectionProperties();
                    PageMargin pageMargin = new PageMargin() { Top = 756, Right = 1008, Bottom = 756, Left = 1512 };
                    sectionProps.Append(pageMargin);
                    body.Append(sectionProps);


                    BillDocument(body, model);
                    ActDocumnet(body, model);
                    ActDocumnet(body, model);

                    mainPart.Document.AppendChild(body);
                }

                mem.Seek(0, SeekOrigin.Begin);

                filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\acts", $"{model.WorkCompletionDate.ToString("MM/dd/yyyy")} - {model.Profile.Lastname} {model.Profile.Firstname}.docx");
                using (FileStream file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    mem.CopyTo(file);
                }
            }

            return filepath;
        }

        private void BillDocument(Body body, ActGenerationViewModel model)
        {
            body.Append(AddParagraph(new Run[] { AddText($"РАХУНОК-ФАКТУРА №1", BoldTextDefault(28)) }));
            body.Append(AddParagraph(new Run[] { AddText($"Від 10 червня 2019 р.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Постачальник: ", BoldTextDefault()),
                        AddText($"ФОП {model.Profile.Lastname} {model.Profile.Firstname} {model.Profile.Middlename}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Адреса: ", BoldTextDefault()),
                        AddText($"{model.Profile.AddressIndex}, {model.Profile.AddressStreet}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Р/Рахунок: ", BoldTextDefault()),
                        AddText($"{model.Profile.AccountNumber}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Банк отримувач: ", BoldTextDefault()),
                        AddText($"{model.Profile.RecipientBank}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"ЄДРПОУ: ", BoldTextDefault()),
                        AddText($"{model.Profile.RegisterNumber}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"МФО: ", BoldTextDefault()),
                        AddText($"{model.Profile.IBAN}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Призначення платежу: ", BoldTextDefault()),
                        AddText($"{model.Profile.PaymentPurpose}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"ІПН: ", BoldTextDefault()),
                        AddText($"{model.Profile.VAT}", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] {
                        AddText($"Платник: ", BoldTextDefault()),
                        AddText("ТОВ «Делойт і Туш»", TextDefault())
                    }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));

            var tableRows = new List<TableRow> { TableHeader() };

            foreach(var service in model.Services)
            {
                tableRows.Add(SingleRow(new List<Run>
                {
                    AddText($"{service.Description}", TextDefault()),
                    AddText($"година", TextDefault()),
                    AddText($"{service.Hours}", TextDefault()),
                    AddText($"{Math.Round(float.Parse(model.CustomUSDRate, CultureInfo.InvariantCulture) * UsdExchangeRate,2)}", TextDefault()),
                    AddText($"{Math.Round(float.Parse(model.CustomUSDRate, CultureInfo.InvariantCulture) * UsdExchangeRate * service.Hours,2)}", TextDefault())
                }));
            }
            tableRows.AddRange(new TableRow[] { TableBottomTotal(model), TableBottom(model) });
            body.Append(AddTable(tableRows));


            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Загальна сума, що підлягає оплаті ", TextDefault()),
                                                 AddText($"{money.Fraction.ToString("### ### ###")}", BoldTextDefault()),
                                                 AddText($" ({money.FractionDescription}) ", TextDefault()),
                                                 AddText($"{money.Cents.ToString()}", BoldTextDefault()),
                                                 AddText($" ({money.CentsDescription}) ", TextDefault()),
                                                 AddText($"копійок", TextDefault()),
                                                 AddText($" без ПДВ.", BoldTextDefault())
            }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"ФОП {model.Profile.Lastname} {model.Profile.Firstname} {model.Profile.Middlename}(_____________________________)", TextDefault()) }));

        }

        private void ActDocumnet(Body body, ActGenerationViewModel model)
        {
            body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

            body.Append(AddParagraph(new Run[] { AddText($"АКТ №1", BoldTextDefault(28)) }, justify: JustificationValues.Center));
            body.Append(AddParagraph(new Run[] { AddText($"приймання-передачі послуг за Договором № {model.Profile.ContractNumber} від {Convert.ToDateTime(model.Profile.ContractDate).ToString("dd MMMM yyyy")} року", BoldTextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Даний акт складений у м.Київ {Convert.ToDateTime(model.WorkCompletionDate).ToString("dd MMMM yyyy")} року.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Повноважним представником ТОВ «Делойт і Туш», що є юридичною особою відповідно до законодавства України, надалі: «Замовник», в особі ... ..., ..., та фізичною особою-підприємцем, {model.Profile.FullnameInAblative}, що є громадянином України, надалі «Виконавець», про те, що Виконавець фактично надав послуги, а Замовник прийняв послуги, перелік і вартість яких зазначені нижче.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));


            var tableRows = new List<TableRow> { TableHeader() };

            foreach (var service in model.Services)
            {
                tableRows.Add(SingleRow(new List<Run>
                {
                    AddText($"{service.Description}", TextDefault()),
                    AddText($"година", TextDefault()),
                    AddText($"{service.Hours}", TextDefault()),
                    AddText($"{Math.Round(float.Parse(model.CustomUSDRate, CultureInfo.InvariantCulture) * UsdExchangeRate,2)}", TextDefault()),
                    AddText($"{Math.Round(float.Parse(model.CustomUSDRate, CultureInfo.InvariantCulture) * UsdExchangeRate * service.Hours,2)}", TextDefault())
                }));
            }

            tableRows.AddRange(new TableRow[] { TableBottomTotal(model), TableBottom(model) });
            body.Append(AddTable(tableRows));

            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Послуги надані вчасно", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Дійсний акт складений на підставі Договору  {model.Profile.ContractNumber} від {Convert.ToDateTime(model.Profile.ContractDate).ToString("dd MMMM yyyy")} року і є невід’ємною частиною цієї Угоди.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Підлягає до виплати ", TextDefault()),                                                 AddText($"{money.Fraction.ToString("### ### ###")}", BoldTextDefault()),
                                                 AddText($" ({money.FractionDescription}) ", TextDefault()),
                                                 AddText($"{money.Cents.ToString()}", BoldTextDefault()),
                                                 AddText($" ({money.CentsDescription}) ", TextDefault()),
                                                 AddText($"копійок", TextDefault()),
                                                 AddText($" без ПДВ.", BoldTextDefault())
            }));
            body.Append(AddParagraph(new Run[] { AddText($"На підставі цього Акту фінансовому відділу ТОВ «Делойт і Туш» зробити розрахунок і виплату винагороди шляхом перерахування на розрахунковий рахунок Виконавця.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"Складено в двох дійсних примірниках українською мовою.", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"", TextDefault()) }));
            body.Append(AddParagraph(new Run[] { AddText($"АКТ ПІДПИСАЛИ", CurveTextDefault()) }));

            body.Append(AddTable(new List<TableRow> {
                        SubscribersHeader(),
                        EmptyRow(2),
                        SubscribersPosition("Директор"),
                        SubscribersFullname("Вахт В.В.","Ковтун А.О.")
                    }, false));

        }

        private Run AddText(string value, RunProperties properties = null)
        {
            var run = new Run();
            run.AppendChild(properties);
            var text = new Text(value) { Space = SpaceProcessingModeValues.Preserve };
            run.AppendChild(text);
            return run;
        }

        private RunProperties TextDefault(int fontSize = 24)
        {
            var properties = new RunProperties();
            properties.Append(new FontSize() { Val = new StringValue($"{fontSize}") });
            properties.Append(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Time New Roman", ComplexScript = "Times New Roman" });
            return properties;
        }

        private RunProperties BoldTextDefault(int fontSize = 24)
        {
            var properties = new RunProperties();
            properties.Append(new Bold());
            properties.Append(new FontSize { Val = new StringValue($"{fontSize}") });
            properties.Append(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Time New Roman", ComplexScript = "Times New Roman" });
            return properties;
        }

        private RunProperties CurveTextDefault(int fontSize = 24)
        {
            var properties = new RunProperties();
            properties.Append(new FontSize { Val = new StringValue($"{fontSize}") });
            properties.Append(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Time New Roman", ComplexScript = "Times New Roman" });
            return properties;
        }

        private Paragraph AddParagraph(Run[] runs, JustificationValues justify = JustificationValues.Left)
        {
            var paragraph = new Paragraph();

            var paragraphProperties = new ParagraphProperties()
            {
                SpacingBetweenLines = new SpacingBetweenLines() { Before = new StringValue("10"), After = new StringValue("10") },
                Justification = new Justification() { Val = justify }
            };

            paragraph.Append(paragraphProperties);
            foreach (var run in runs)
            {
                paragraph.Append(run);
            }
            return paragraph;
        }

        private Table AddTable(IEnumerable<TableRow> rows, bool borderTable = true)
        {
            var table = new Table();
            SetTableStyle(table, borderTable);
            foreach (var row in rows)
            {
                table.Append(row);
            }
            return table;
        }

        private TableRow TableHeader()
        {
            return SingleRow(new List<Run>
            {
                AddText($"Опис послуги", BoldTextDefault()),
                AddText($"Од. виміру", BoldTextDefault()),
                AddText($"Клк.", BoldTextDefault()),
                AddText($"Ціна", BoldTextDefault()),
                AddText($"Сума", BoldTextDefault())
            });
        }

        private TableRow TableBottomTotal(ActGenerationViewModel model)
        {
            return SingleRow(new List<Run>
            {
                AddText($"Всього", BoldTextDefault()),
                AddText($"година", TextDefault()),
                AddText($"{model.Services.Sum(service => service.Hours)}", BoldTextDefault()),
                AddText($"", TextDefault()),
                AddText($"{Math.Round(model.TotalPay,2)}", BoldTextDefault())
            });
        }

        private TableRow TableBottom(ActGenerationViewModel model)
        {
            return SingleRow(new List<Run>
            {
                AddText($"Загальна сума без ПДВ", BoldTextDefault()),
                AddText($"", TextDefault()),
                AddText($"", BoldTextDefault()),
                AddText($"", BoldTextDefault()),
                AddText($"{Math.Round(model.TotalPay, 2)}", BoldTextDefault())
            });
        }

        private TableRow SingleRow(IEnumerable<Run> data)
        {
            var row = new TableRow();
            foreach (var value in data)
            {
                row.Append(new TableCell(AddParagraph(new Run[] { value })));
            }
            return row;
        }

        private TableRow EmptyRow(int cellsQuantity)
        {
            var runs = new List<Run>();
            for (int i = 0; i < cellsQuantity; i++)
            {
                runs.Add(AddText("", TextDefault()));
            }
            return SingleRow(runs);
        }

        private TableRow SubscribersHeader()
        {
            return SingleRow(new List<Run>
            {
                AddText($"Замовник",BoldTextDefault()),
                AddText($"Виконавець", BoldTextDefault())
            });
        }

        private TableRow SubscribersPosition(string client)
        {
            return SingleRow(new List<Run>
            {
                AddText($"{client}",TextDefault()),
                AddText($"ФОП", TextDefault())
            });
        }

        private TableRow SubscribersFullname(string client, string perfomer)
        {
            return SingleRow(new List<Run>
            {
                AddText($"{client}",TextDefault()),
                AddText($"{perfomer}", TextDefault())
            });
        }

        private void SetTableStyle(Table table, bool bordered = true)
        {
            TableProperties properties = new TableProperties();

            if (bordered)
            {
                TableBorders borders = new TableBorders();

                borders.TopBorder = new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
                borders.BottomBorder = new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
                borders.LeftBorder = new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
                borders.RightBorder = new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
                borders.InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single };
                borders.InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single };

                properties.Append(borders);
            }

            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            properties.Append(tableWidth);

            table.Append(properties);
        }
    }
}
