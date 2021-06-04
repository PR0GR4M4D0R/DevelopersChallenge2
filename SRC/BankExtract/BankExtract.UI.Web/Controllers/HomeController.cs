using BankExtract.UI.Web.Utils;
using BankExtract.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace BankExtract.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        #region " CONSTANTS "

        /// <summary>
        /// Constant to cache the bank list key to be retrieved.
        /// </summary>
        private const string KEY_LIST_OF_BANKS = "ListOfBanks";

        #endregion " CONSTANTS "

        #region " PROPERTIES "

        /// <summary>
        /// Gets the list of cached banks.
        /// </summary>
        private List<DtoConcept<int?>> ListOfBanks
        {
            get
            {
                var listOfBanks = new List<DtoConcept<int?>>();
                if (!TempData.ContainsKey(KEY_LIST_OF_BANKS))
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BankExtract.UI.Web.Models.banks.txt"))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            do
                            {
                                var row = sr.ReadLine();
                                var arrayBank = row.Split('\t');
                                var code = Convert.ToInt32(arrayBank.First());
                                var description = arrayBank.Last();
                                listOfBanks.Add(new DtoConcept<int?>
                                {
                                    Code = code,
                                    Description = description
                                });
                            }
                            while (!(sr.EndOfStream));
                        }

                    }

                    TempData[KEY_LIST_OF_BANKS] = listOfBanks;
                }

                return TempData.Peek(KEY_LIST_OF_BANKS) as List<DtoConcept<int?>>;
            }
        }

        #endregion " PROPERTIES "

        #region " PUBLIC METHODS "

        /// <summary>
        /// Gets the splash screen.
        /// </summary>
        /// <returns>Return the initial view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Processes OFX type files to transform into object.
        /// </summary>
        /// <param name="dados">Contains information from the files sent by the form.</param>
        /// <returns>Returns the list of type OFX in list format for filling the screen.</returns>
        [HttpPost]
        public ActionResult Upload(Extract dados)
        {
            var checkDuplicateNextFile = false;
            if (ModelState.IsValid)
            {
                if (dados.Files != null && dados.Files.Any())
                {
                    var extract = new Extract();
                    foreach (var file in dados.Files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            if (file.FileName.EndsWith(".ofx"))
                            {
                                using (StreamReader sr = new StreamReader(file.InputStream))
                                {
                                    bool openTransaction = false;
                                    var movement = new Movement();
                                    var bank = new DtoConcept<int?>();
                                    do
                                    {
                                        string row = sr.ReadLine();
                                        if (row.Contains("<BANKID>"))
                                        {
                                            var fileBankCode = Convert.ToInt32(row.Split('>')[1]);
                                            bank = ListOfBanks.Where(x => x.Code == fileBankCode).First();
                                            continue;
                                        }
                                        if (row.Contains("<STMTTRN>"))
                                        {
                                            openTransaction = true;
                                            continue;
                                        }
                                        else if (openTransaction)
                                        {
                                            var values = row.Split('>');
                                            var tag = values[0].Trim().Split('<')[1];
                                            var value = values[1];
                                            switch (tag)
                                            {
                                                case "TRNTYPE":
                                                    movement.Type = value;
                                                    continue;
                                                case "DTPOSTED":
                                                    movement.DateMovement = value.ConvertStringOFXInDateTime();
                                                    continue;
                                                case "TRNAMT":
                                                    movement.Value = value.ConvertStringOFXInDecimal();
                                                    continue;
                                                case "MEMO":
                                                    movement.Description = value;
                                                    openTransaction = false;
                                                    continue;
                                            }
                                        }
                                        else
                                        {
                                            if (movement.PopulatedMovement())
                                            {
                                                if (!checkDuplicateNextFile || (checkDuplicateNextFile && !extract.ExistMovementEquals(movement)))
                                                {
                                                    movement.Bank = bank;
                                                    extract.Movements.Add(movement);
                                                    movement = new Movement();
                                                }
                                            }
                                        }

                                        if (sr.EndOfStream)
                                        {
                                            checkDuplicateNextFile = true;
                                        }

                                    } while (!(sr.EndOfStream));
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Files", "Selecione um arquivo no formato .ofx");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Files", "Adicione um arquivo!");
                        }
                    }

                    return View("Index", extract);
                }
                else
                {
                    ModelState.AddModelError("Files", "Arquivo inválido!");
                }
            }

            return View("Index", new Extract());
        }

        #endregion " PUBLIC METHODS "
    }
}