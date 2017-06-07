using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osmutil
{
    public class FindMovers : IOperation
    {
        private List<string> _sectionFilter;
        private Service _service;

        public FindMovers(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }


        public void DoIt(Action<string, bool> feedback, bool dryRun)
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

            feedback($"Start of next term is {startOfNextTerm.ToShortDateString()}", true);
            feedback($"End of next term is {endOfNextTerm.ToShortDateString()}", true);

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
                var willBes = new List<string>();
                foreach (var m in members)
                {
                    DateTime dob = m.data.dob;
                    double age = (date - dob).TotalDays / 365.25;

                    if (age < 16)
                    {
                        bool willBeTransferAgeAtStartOfNextTerm = (startOfNextTerm - dob).TotalDays / 365.25 >= transferAgeForSection;
                        bool willBeTransferAgeAtEndOfNextTerm = (endOfNextTerm - dob).TotalDays / 365.25 >= transferAgeForSection;

                        string reportString = "";
                        if (willBeTransferAgeAtStartOfNextTerm)
                        {
                            reportString += $"{Name(m, s, dob)} will be {GetAgeAt(dob, startOfNextTerm)} at the start of next term";
                            feedback(reportString, true);
                        }
                        else if (willBeTransferAgeAtEndOfNextTerm)
                        {
                            reportString += $"   {Name(m, s, dob)} will be {GetAgeAt(dob, endOfNextTerm)} at the end of next term";
                            willBes.Add(reportString);
                        }
                    }
                }
                foreach (var wb in willBes)
                {
                    feedback(wb, true);
                }
                feedback("", true);
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
        private string Name(MemberDetails m, GroupSection s, DateTime dob)
        {
            var str = $"{s.sectionname} : {m.data.firstname} {m.data.lastname} (dob: {dob.ToShortDateString()})";
            if (s.section == "waiting")
            {
                str += $", joined WL {m.data.started.ToShortDateString()}";
            }
            str += ")";

            return str;
        }
    }
}
