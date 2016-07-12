using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class FindMovers
    {
        private List<string> _sectionFilter;
        private Service _service;

        public FindMovers(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt()
        {
            // When determing if a member has a birthday next term, these dates will be used.
            // Autumn Term: 1 September to 31 December
            // Spring Term: 1 Jan to 31 March
            // Summer Term: 1 April to 31 August
            var date = DateTime.Now.Date;
            DateTime startOfNextTerm;
            DateTime endOfNextTerm;
            if (date.Month > 8)
            {
                startOfNextTerm = new DateTime(date.Year + 1, 1, 1);
                endOfNextTerm = new DateTime(date.Year + 1, 3, 31);
            }
            else if (date.Month > 3)
            {
                startOfNextTerm = new DateTime(date.Year, 9, 1);
                endOfNextTerm = new DateTime(date.Year, 12, 31);
            }
            else
            {
                startOfNextTerm = new DateTime(date.Year, 4, 1);
                endOfNextTerm = new DateTime(date.Year, 8, 31);
            }

            Console.WriteLine($"Start of next term is {startOfNextTerm.ToShortDateString()}");
            Console.WriteLine($"End of next term is {endOfNextTerm.ToShortDateString()}");

            foreach (var s in _service.GetRequiredSections(_sectionFilter))
            {
                double transferAgeForSection;
                switch (s.section)
                {
                    case "beavers":
                        transferAgeForSection = 8;
                        break;
                    case "cubs":
                        transferAgeForSection = 10.5;
                        break;
                    case "scouts":
                        transferAgeForSection = 14;
                        break;
                    case "waiting":
                        transferAgeForSection = 6;
                        break;
                    case "adults":
                        transferAgeForSection = 100;
                        break;
                    default:
                        throw new ApplicationException($"Invalid section : {s.section}");
                }

                var termId = _service.GetLatestTermIdForSection(s.sectionid);
                var members = _service.GetMembers(s.sectionid, termId).items.Select(m => _service.GetMemberDetails(m.sectionid, termId, m.scoutid)).OrderBy(m => m.data.started);
                foreach (var m in members)
                {
                    DateTime dob = m.data.dob;
                    double age = (date - dob).TotalDays / 365.25;

                    if (age < 16)
                    {
                        bool hasReachedTransferAge = age >= transferAgeForSection;
                        bool willBeTransferAgeAtStartOfNextTerm = (startOfNextTerm - dob).TotalDays / 365.25 >= transferAgeForSection;
                        bool willBeTransferAgeAtEndOfNextTerm = (endOfNextTerm - dob).TotalDays / 365.25 >= transferAgeForSection;

                        string reportString = "";
                        if (hasReachedTransferAge)
                        {
                            reportString += $"{Name(m, s)} is currently {GetAgeAt(dob, date)}";
                        }
                        else if (willBeTransferAgeAtStartOfNextTerm)
                        {
                            reportString += $"{Name(m, s)} will be {GetAgeAt(dob, startOfNextTerm)} at the start of next term";
                        }
                        else if (willBeTransferAgeAtEndOfNextTerm)
                        {
                            reportString += $"{Name(m, s)} will be {GetAgeAt(dob, endOfNextTerm)} at the end of next term";
                        }

                        if (reportString != "")
                        {
                            reportString += $" (dob: {dob.ToShortDateString()}";
                            if (s.section == "waiting")
                            {
                                reportString += $", joined WL {m.data.started.ToShortDateString()}";
                            }
                            reportString += ")";
                            Console.WriteLine(reportString);
                        }
                    }
                }
            }
        }
        private string GetAgeAt(DateTime Bday, DateTime Cday)
        {
            if ((Cday.Year - Bday.Year) > 0 ||
                (((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
                  ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                if (Cday.Month > Bday.Month)
                {
                    return $"{ Cday.Year - Bday.Year} / { Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth)}";
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        return $"{ Cday.Year - Bday.Year} / 0";
                    }
                    else
                    {
                        return $"{ (Cday.Year - 1) - Bday.Year} / 11";
                    }
                }
                else
                {
                    return $"{ (Cday.Year - 1) - Bday.Year} / {Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth)}";
                }
            }
            else
            {
                throw new ArgumentException("Birthday date must be earlier than current date");
            }
        }
        private string Name(MemberDetails m, GroupSection s)
        {
            return $"{m.data.firstname} {m.data.lastname} ({s.sectionname})";
        }
    }
}
