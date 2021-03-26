using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Helpers
{
    public class DescriptionNode
    {
         static Dictionary<int, string> nodeDescription = new Dictionary<int, string>();

         static DescriptionNode()
        {
            nodeDescription.Add(1, "Điểm bắt đầu");
            nodeDescription.Add(2, "Sảnh trung tâm tầng 1");
            nodeDescription.Add(3, "Hành lang trước phòng 1 & 2");
            nodeDescription.Add(4, "Phòng 2");
            nodeDescription.Add(5, "Phòng 4");
            nodeDescription.Add(6, "Phòng 6");
            nodeDescription.Add(7, "Hành lang trước phòng 6 & 7");
            nodeDescription.Add(8, "Phòng 7");
            nodeDescription.Add(9, "Phòng 8");
            nodeDescription.Add(10, "Phòng 9");
            nodeDescription.Add(11, "Phòng 1");
            nodeDescription.Add(12, "Phòng 3");
            nodeDescription.Add(13, "Phòng 5");
            nodeDescription.Add(14, "Hành lang trước cửa phòng 5");
            nodeDescription.Add(15, "Hành lang trước cửa phòng 9 & 10");
            nodeDescription.Add(16, "Phòng 10");
            nodeDescription.Add(17, "Phòng 11");
            nodeDescription.Add(18, "Phòng 12");
            nodeDescription.Add(19, "Phòng 13");
            nodeDescription.Add(20, "Phòng 14");
            nodeDescription.Add(21, "Phòng 15");
            nodeDescription.Add(22, "Phòng 16");
            nodeDescription.Add(23, "Phòng 17");
            nodeDescription.Add(24, "Phòng 18");
            nodeDescription.Add(25, "Hành lang trước cửa phòng 9 & 10");
            nodeDescription.Add(26, "Phòng 20");
            nodeDescription.Add(27, "Phòng 21");
            nodeDescription.Add(28, "Phòng 19");
            nodeDescription.Add(29, "Chân cầu thang trung tâm tầng 1");
            nodeDescription.Add(30, "Chân cầu thang phụ tầng 1");
            nodeDescription.Add(31, "Sảnh cầu thang phụ tầng 1");
            nodeDescription.Add(32, "Hành lang phòng 22 & 23");
            nodeDescription.Add(33, "Phòng 22");
            nodeDescription.Add(34, "Phòng 23");
            nodeDescription.Add(35, "Hành lang phòng 24 & 25");
            nodeDescription.Add(36, "Phòng 24");
            nodeDescription.Add(37, "Phòng 25");
            nodeDescription.Add(38, "Hành lang phòng 26 & 27");
            nodeDescription.Add(39, "Phòng 26");
            nodeDescription.Add(40, "Phòng 27");
            nodeDescription.Add(41, "Cầu thang trung tâm tầng 2");
            nodeDescription.Add(42, "Sảnh trung tâm tầng 2");
            nodeDescription.Add(43, "Hành lang trung tâm cạnh ban công");
            nodeDescription.Add(44, "Hành lang trước phòng 46");
            nodeDescription.Add(45, "Phòng 49");
            nodeDescription.Add(46, "Phòng 47");
            nodeDescription.Add(47, "Phòng 48");
            nodeDescription.Add(48, "Phòng 50");
            nodeDescription.Add(49, "Hành lang trước phòng 33");
            nodeDescription.Add(50, "Cầu thang phụ tầng 2");
            nodeDescription.Add(51, "Hành lang trước phòng 37 & 38");
            nodeDescription.Add(52, "Phòng 37");
            nodeDescription.Add(53, "Phòng 38");
            nodeDescription.Add(54, "Hành lang trước phòng 39 & 40");
            nodeDescription.Add(55, "Phòng 39");
            nodeDescription.Add(56, "Phòng 40");

            nodeDescription.Add(57, "Hành lang phòng 41 & 42");
            nodeDescription.Add(58, "Phòng 42");
            nodeDescription.Add(59, "Phòng 44");
            nodeDescription.Add(60, "Phòng 46");
            nodeDescription.Add(61, "Phòng 41");
            nodeDescription.Add(62, "Phòng 43");
            nodeDescription.Add(63, "Phòng 45");
            nodeDescription.Add(64, "Hành lang phòng 33 & 36");
            nodeDescription.Add(65, "Phòng 33");
            nodeDescription.Add(66, "Phòng 32");
            nodeDescription.Add(67, "Phòng 31");
            nodeDescription.Add(68, "Phòng  28");
            nodeDescription.Add(69, "Phòng 29");
            nodeDescription.Add(70, "Phòng 30");
            nodeDescription.Add(71, "Hành lang trước phòng 30");
            nodeDescription.Add(72, "Phòng 36");
            nodeDescription.Add(73, "Hành lang trước phòng 36");
            nodeDescription.Add(74, "Phòng 35");
            nodeDescription.Add(75, "Phòng 34");
            nodeDescription.Add(76, "Hành lang trước phòng 45 & 51");
            nodeDescription.Add(77, "Phòng 51");
            nodeDescription.Add(78, "Phòng 52");
            nodeDescription.Add(79, "Phòng 53");
            nodeDescription.Add(80, "Phòng 54");
            nodeDescription.Add(81, "Phòng 55");
            nodeDescription.Add(82, "Phòng 56");
        }
       
        public static string GetDescription(int key)
        {
            return nodeDescription[key];
        }

        public static Dictionary<int, string> ReturnDict()
        {
            return nodeDescription;
        }

    }

    
}
