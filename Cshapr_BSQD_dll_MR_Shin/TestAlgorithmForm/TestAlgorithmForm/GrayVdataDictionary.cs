using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TestAlgorithmForm
{
    class GrayVdataDictionary
    {
        

        Form1 f1 = (Form1)Application.OpenForms["Form1"];
        public Dictionary<int, double> keyValuePairs = new Dictionary<int, double>();

        private GrayVdataDictionary() { }
        private static GrayVdataDictionary Instance;
        public static GrayVdataDictionary getInstance()
        {
            if (Instance == null)
                Instance = new GrayVdataDictionary();
            return Instance;
        }

        public Color Get_Color()
        {
            if (f1.radioButton_R.Checked)
                return Color.Red;
            if (f1.radioButton_G.Checked)
                return Color.Green;
            if (f1.radioButton_B.Checked)
                return Color.Blue;

            return Color.Black;
        }

        public void Updata_Vdata_Gray_DataSet()
        {
            keyValuePairs.Clear();

            if (f1.radioButton_DataSet1.Checked)
            {
                /*
B0_G255	339	273	432	339	273	432	3.92622222222222	4.20488888888889	3.53355555555556
B0_G191	433	427	421	433	427	421	4.25447930283224	4.4761045751634	3.91293464052288
B0_G127	421	407	404	421	407	404	4.58496437267718	4.76580648468538	4.29764486949464
B0_G95	463	452	453	463	452	453	4.76466562940536	4.92887262110727	4.50332832828887
B0_G63	406	379	385	406	379	385	4.97806087177007	5.1306324170191	4.74447445239244
B0_G47	468	450	456	468	450	456	5.10602928379806	5.25576062948796	4.89151983999352
B0_G31	411	371	385	411	371	385	5.2756618299747	5.41781192104599	5.08134206762399
B0_G23	465	439	447	465	439	447	5.38582068466013	5.5251196196489	5.21034097583766
B0_G15	403	350	366	403	350	366	5.53429566271441	5.65776385819971	5.37360521904558
B0_G7	386	327	348	386	327	348	5.77049735096231	5.84576191610364	5.60910459918016
B0_G2	205	299	299	205	299	299	6.20755166855124	5.98452542827253	5.81788680941546
 */

                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 3.926222222);
                    keyValuePairs.Add(191, 4.254479303);
                    keyValuePairs.Add(127, 4.584964373);
                    keyValuePairs.Add(95, 4.764665629);
                    keyValuePairs.Add(63, 4.978060872);
                    keyValuePairs.Add(47, 5.106029284);
                    keyValuePairs.Add(31, 5.27566183);
                    keyValuePairs.Add(23, 5.385820685);
                    keyValuePairs.Add(15, 5.534295663);
                    keyValuePairs.Add(7, 5.770497351);
                    keyValuePairs.Add(2, 6.207551669);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 4.20488888888889);
                    keyValuePairs.Add(191, 4.4761045751634);
                    keyValuePairs.Add(127, 4.76580648468538);
                    keyValuePairs.Add(95, 4.92887262110727);
                    keyValuePairs.Add(63, 5.1306324170191);
                    keyValuePairs.Add(47, 5.25576062948796);
                    keyValuePairs.Add(31, 5.41781192104599);
                    keyValuePairs.Add(23, 5.5251196196489);
                    keyValuePairs.Add(15, 5.65776385819971);
                    keyValuePairs.Add(7, 5.84576191610364);
                    keyValuePairs.Add(2, 5.98452542827253);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 3.53355555555556);
                    keyValuePairs.Add(191, 3.91293464052288);
                    keyValuePairs.Add(127, 4.29764486949464);
                    keyValuePairs.Add(95, 4.50332832828887);
                    keyValuePairs.Add(63, 4.74447445239244);
                    keyValuePairs.Add(47, 4.89151983999352);
                    keyValuePairs.Add(31, 5.08134206762399);
                    keyValuePairs.Add(23, 5.21034097583766);
                    keyValuePairs.Add(15, 5.37360521904558);
                    keyValuePairs.Add(7, 5.60910459918016);
                    keyValuePairs.Add(2, 5.81788680941546);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_DataSet2.Checked)
            {
                /*
B2_G255	341	283	430	341	283	430	4.34906308506308	4.55968547008547	4.02586666666667
B2_G191	450	443	441	450	443	441	4.57057973137973	4.7525542441098	4.27882002122787
B2_G127	434	420	421	434	420	421	4.8223285461573	4.98197984458319	4.56684674007696
B2_G95	468	458	460	468	458	460	4.9692309782788	5.11795016364808	4.73324967647454
B2_G63	415	389	397	415	389	397	5.15029676670763	5.29496812620426	4.93880624496567
B2_G47	470	455	460	470	455	460	5.26410344740641	5.40440178512147	5.06815852755598
B2_G31	418	381	393	418	381	393	5.40844362780486	5.54901054869064	5.23809191840992
B2_G23	467	447	453	467	447	453	5.50839313072801	5.64232228464236	5.3512949806707
B2_G15	406	367	378	406	367	378	5.64695948705328	5.75896195458202	5.49767825083552
B2_G7	385	384	380	385	384	380	5.87448125591447	5.89204964047801	5.6869434326171
B2_G4	313	411	385	313	411	385	6.14402853007312	5.97084940528786	5.82240777416956
 */

                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 4.34906308506308);
                    keyValuePairs.Add(191, 4.57057973137973);
                    keyValuePairs.Add(127, 4.8223285461573);
                    keyValuePairs.Add(95, 4.9692309782788);
                    keyValuePairs.Add(63, 5.15029676670763);
                    keyValuePairs.Add(47, 5.26410344740641);
                    keyValuePairs.Add(31, 5.40844362780486);
                    keyValuePairs.Add(23, 5.50839313072801);
                    keyValuePairs.Add(15, 5.64695948705328);
                    keyValuePairs.Add(7, 5.87448125591447);
                    keyValuePairs.Add(4, 6.14402853007312);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 4.55968547008547);
                    keyValuePairs.Add(191, 4.7525542441098);
                    keyValuePairs.Add(127, 4.98197984458319);
                    keyValuePairs.Add(95, 5.11795016364808);
                    keyValuePairs.Add(63, 5.29496812620426);
                    keyValuePairs.Add(47, 5.40440178512147);
                    keyValuePairs.Add(31, 5.54901054869064);
                    keyValuePairs.Add(23, 5.64232228464236);
                    keyValuePairs.Add(15, 5.75896195458202);
                    keyValuePairs.Add(7, 5.89204964047801);
                    keyValuePairs.Add(4, 5.97084940528786);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 4.02586666666667);
                    keyValuePairs.Add(191, 4.27882002122787);
                    keyValuePairs.Add(127, 4.56684674007696);
                    keyValuePairs.Add(95, 4.73324967647454);
                    keyValuePairs.Add(63, 4.93880624496567);
                    keyValuePairs.Add(47, 5.06815852755598);
                    keyValuePairs.Add(31, 5.23809191840992);
                    keyValuePairs.Add(23, 5.3512949806707);
                    keyValuePairs.Add(15, 5.49767825083552);
                    keyValuePairs.Add(7, 5.6869434326171);
                    keyValuePairs.Add(4, 5.82240777416956);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_DataSet3.Checked)
            {
                /*
B4_G255	336	283	421	336	283	421	4.77163071496405	4.93453219373219	4.51037362637363
B4_G191	460	452	454	460	452	454	4.92710429611541	5.07616949500915	4.68213434205591
B4_G127	446	429	434	446	429	434	5.10874418259126	5.25404379055874	4.89255155867907
B4_G95	471	461	464	471	461	464	5.22816367526228	5.36631677150539	5.02675764918205
B4_G63	421	397	404	421	397	404	5.37743804110106	5.5100261871171	5.19808457322841
B4_G47	474	457	464	474	457	464	5.46848374220288	5.60428286315247	5.30424368778643
B4_G31	421	391	399	421	391	399	5.59890055729469	5.7194854671957	5.45105948451561
B4_G23	463	459	459	463	459	459	5.69625178837147	5.78897794709678	5.54281935747134
B4_G15	397	407	398	397	407	398	5.83010973110204	5.85847042699785	5.65046074690019
B4_G7	395	458	437	395	458	437	6.01299187776354	5.91491218576366	5.75222239086394
B4_G5	374	476	458	374	476	458	6.18004673735202	5.94832672197286	5.81457181028367
*/
                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 4.77163071496405);
                    keyValuePairs.Add(191, 4.92710429611541);
                    keyValuePairs.Add(127, 5.10874418259126);
                    keyValuePairs.Add(95, 5.22816367526228);
                    keyValuePairs.Add(63, 5.37743804110106);
                    keyValuePairs.Add(47, 5.46848374220288);
                    keyValuePairs.Add(31, 5.59890055729469);
                    keyValuePairs.Add(23, 5.69625178837147);
                    keyValuePairs.Add(15, 5.83010973110204);
                    keyValuePairs.Add(7, 6.01299187776354);
                    keyValuePairs.Add(5, 6.18004673735202);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 4.93453219373219);
                    keyValuePairs.Add(191, 5.07616949500915);
                    keyValuePairs.Add(127, 5.25404379055874);
                    keyValuePairs.Add(95, 5.36631677150539);
                    keyValuePairs.Add(63, 5.5100261871171);
                    keyValuePairs.Add(47, 5.60428286315247);
                    keyValuePairs.Add(31, 5.7194854671957);
                    keyValuePairs.Add(23, 5.78897794709678);
                    keyValuePairs.Add(15, 5.85847042699785);
                    keyValuePairs.Add(7, 5.91491218576366);
                    keyValuePairs.Add(5, 5.94832672197286);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 4.51037362637363);
                    keyValuePairs.Add(191, 4.68213434205591);
                    keyValuePairs.Add(127, 4.89255155867907);
                    keyValuePairs.Add(95, 5.02675764918205);
                    keyValuePairs.Add(63, 5.19808457322841);
                    keyValuePairs.Add(47, 5.30424368778643);
                    keyValuePairs.Add(31, 5.45105948451561);
                    keyValuePairs.Add(23, 5.54281935747134);
                    keyValuePairs.Add(15, 5.65046074690019);
                    keyValuePairs.Add(7, 5.75222239086394);
                    keyValuePairs.Add(5, 5.81457181028367);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_DataSet4.Checked)
            {
                /*
B6_G255	336	283	423	336	283	423	4.80975444308778	4.96986894586895	4.54692497625831
B6_G191	462	453	456	462	453	456	4.95657543384559	5.10672350484987	4.71036655928595
B6_G127	444	427	433	444	427	433	5.1412571262684  	5.28614270016021	4.92132565067688
B6_G95	470	457	465	470	457	465	5.26155541268362	5.40508801490837	5.05108324084432
B5_G63	441	416	429	441	416	429	5.34664444453829	5.49539834647641	5.15263265923623
B6_G47	446	421	435	446	421	435	5.51128667818489	5.65685731864432	5.33268013064724
B6_G31	388	349	371	388	349	371	5.65819820974646	5.78602449637864	5.48429905394071
B6_G23	457	452	462	457	452	462	5.76211872537087	5.85837969983496	5.56864078165658
B6_G15	380	387	405	380	387	405	5.91030168283531	5.93809305957498	5.66675258736687
B6_G8	356	443	448	356	443	448	6.13227166439635	6.00128889140733	5.75274161445219
B6_G4	374	476	458	374	476	458	6.26907017533598	6.02949613022496	5.81618038173732
*/
                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 4.80975444308778);
                    keyValuePairs.Add(191, 4.95657543384559);
                    keyValuePairs.Add(127, 5.1412571262684);
                    keyValuePairs.Add(95, 5.26155541268362);
                    keyValuePairs.Add(63, 5.34664444453829);
                    keyValuePairs.Add(47, 5.51128667818489);
                    keyValuePairs.Add(31, 5.65819820974646);
                    keyValuePairs.Add(23, 5.76211872537087);
                    keyValuePairs.Add(15, 5.91030168283531);
                    keyValuePairs.Add(8, 6.13227166439635);
                    keyValuePairs.Add(4, 6.26907017533598);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 4.96986894586895);
                    keyValuePairs.Add(191, 5.10672350484987);
                    keyValuePairs.Add(127, 5.28614270016021);
                    keyValuePairs.Add(95, 5.40508801490837);
                    keyValuePairs.Add(63, 5.49539834647641);
                    keyValuePairs.Add(47, 5.65685731864432);
                    keyValuePairs.Add(31, 5.78602449637864);
                    keyValuePairs.Add(23, 5.85837969983496);
                    keyValuePairs.Add(15, 5.93809305957498);
                    keyValuePairs.Add(8, 6.00128889140733);
                    keyValuePairs.Add(4, 6.02949613022496);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 4.54692497625831);
                    keyValuePairs.Add(191, 4.71036655928595);
                    keyValuePairs.Add(127, 4.92132565067688);
                    keyValuePairs.Add(95, 5.05108324084432);
                    keyValuePairs.Add(63, 5.15263265923623);
                    keyValuePairs.Add(47, 5.33268013064724);
                    keyValuePairs.Add(31, 5.48429905394071);
                    keyValuePairs.Add(23, 5.56864078165658);
                    keyValuePairs.Add(15, 5.66675258736687);
                    keyValuePairs.Add(8, 5.75274161445219);
                    keyValuePairs.Add(4, 5.81618038173732);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_DataSet5.Checked)
            {
                /*
B8_G255	338	283	428	338	283	428	4.89741066341066	5.05647863247863	4.63711762311762
B8_G191	463	451	457	463	451	457	5.03555311355311	5.19201248134581	4.7920175010175
B8_G127	444	417	437	444	417	437	5.21325351204109	5.38353155405879	4.98555798026713
B8_G95	467	449	464	467	449	464	5.33747388155316	5.51133296985105	5.11472459128661
B8_G63	408	369	399	408	369	399	5.50404210430797	5.67623802248622	5.29335926610079
B8_G47	462	442	458	462	442	458	5.61454904790665	5.77902186022489	5.40715298923963
B8_G31	399	360	400	399	360	400	5.75662940396211	5.90117076884186	5.53168196927836
B8_G23	442	458	475	442	458	475	5.87820113719408	5.95683661936988	5.5922187538904
B8_G15	339	409	440	339	409	440	6.05967778245341	6.00830127363164	5.6510739611521
B8_G7	319	426	472	328	435	448	6.27379104223866	6.07222181838596	5.74232252272308
B8_G4	374	476	458	374	476	458	6.37679145803337	6.09728934287586	5.80964153375063
*/
                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 4.89741066341066);
                    keyValuePairs.Add(191, 5.03555311355311);
                    keyValuePairs.Add(127, 5.21325351204109);
                    keyValuePairs.Add(95, 5.33747388155316);
                    keyValuePairs.Add(63, 5.50404210430797);
                    keyValuePairs.Add(47, 5.61454904790665);
                    keyValuePairs.Add(31, 5.75662940396211);
                    keyValuePairs.Add(23, 5.87820113719408);
                    keyValuePairs.Add(15, 6.05967778245341);
                    keyValuePairs.Add(7, 6.27379104223866);
                    keyValuePairs.Add(4, 6.37679145803337);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 5.05647863247863);
                    keyValuePairs.Add(191, 5.19201248134581);
                    keyValuePairs.Add(127, 5.38353155405879);
                    keyValuePairs.Add(95, 5.51133296985105);
                    keyValuePairs.Add(63, 5.67623802248622);
                    keyValuePairs.Add(47, 5.77902186022489);
                    keyValuePairs.Add(31, 5.90117076884186);
                    keyValuePairs.Add(23, 5.95683661936988);
                    keyValuePairs.Add(15, 6.00830127363164);
                    keyValuePairs.Add(7, 6.07222181838596);
                    keyValuePairs.Add(4, 6.09728934287586);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 4.63711762311762);
                    keyValuePairs.Add(191, 4.7920175010175);
                    keyValuePairs.Add(127, 4.98555798026713);
                    keyValuePairs.Add(95, 5.11472459128661);
                    keyValuePairs.Add(63, 5.29335926610079);
                    keyValuePairs.Add(47, 5.40715298923963);
                    keyValuePairs.Add(31, 5.53168196927836);
                    keyValuePairs.Add(23, 5.5922187538904);
                    keyValuePairs.Add(15, 5.6510739611521);
                    keyValuePairs.Add(7, 5.74232252272308);
                    keyValuePairs.Add(4, 5.80964153375063);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_DataSet6.Checked)
            {
                /*
B10_G255	342	283	434	342	283	434	5.09311246777913	5.24701994301994	4.85312115045448
B10_G191	462	445	461	462	445	461	5.2211427888661	5.38149078130777	4.9837643352371
B10_G127	442	413	435	442	413	435	5.38699559276317	5.55962668978236	5.16611831263694
B10_G95	466	449	462	466	449	462	5.5017095098608	5.67276514504519	5.28919974927422
B10_G63	407	371	400	407	371	400	5.65211220116657	5.81510062102101	5.44493544461117
B10_G47	461	451	468	461	451	468	5.75368181526343	5.89465109737769	5.52952940235736
B10_G31	383	392	427	383	392	427	5.91213041325453	5.97287573246176	5.61018875741768
B10_G23	425	484	492	425	484	492	6.04315521543947	6.00035327492989	5.64143503577851
B10_G20	329	430	461	329	430	461	6.18941545973894	6.05530835986613	5.69241580573566
B10_G7	319	426	472	319	426	472	6.37795405590623	6.12812663014389	5.75028951497758
B10_G4	374	476	458	374	476	458	6.46203539599386	6.1531328123429	5.82294756871285
*/
                if (f1.radioButton_R.Checked)
                {
                    keyValuePairs.Add(255, 5.09311246777913);
                    keyValuePairs.Add(191, 5.2211427888661);
                    keyValuePairs.Add(127, 5.38699559276317);
                    keyValuePairs.Add(95, 5.5017095098608);
                    keyValuePairs.Add(63, 5.65211220116657);
                    keyValuePairs.Add(47, 5.75368181526343);
                    keyValuePairs.Add(31, 5.91213041325453);
                    keyValuePairs.Add(23, 6.04315521543947);
                    keyValuePairs.Add(20, 6.18941545973894);
                    keyValuePairs.Add(7, 6.37795405590623);
                    keyValuePairs.Add(4, 6.46203539599386);
                }
                else if (f1.radioButton_G.Checked)
                {
                    keyValuePairs.Add(255, 5.24701994301994);
                    keyValuePairs.Add(191, 5.38149078130777);
                    keyValuePairs.Add(127, 5.55962668978236);
                    keyValuePairs.Add(95, 5.67276514504519);
                    keyValuePairs.Add(63, 5.81510062102101);
                    keyValuePairs.Add(47, 5.89465109737769);
                    keyValuePairs.Add(31, 5.97287573246176);
                    keyValuePairs.Add(23, 6.00035327492989);
                    keyValuePairs.Add(20, 6.05530835986613);
                    keyValuePairs.Add(7, 6.12812663014389);
                    keyValuePairs.Add(4, 6.1531328123429);
                }
                else if (f1.radioButton_B.Checked)
                {
                    keyValuePairs.Add(255, 4.85312115045448);
                    keyValuePairs.Add(191, 4.9837643352371);
                    keyValuePairs.Add(127, 5.16611831263694);
                    keyValuePairs.Add(95, 5.28919974927422);
                    keyValuePairs.Add(63, 5.44493544461117);
                    keyValuePairs.Add(47, 5.52952940235736);
                    keyValuePairs.Add(31, 5.61018875741768);
                    keyValuePairs.Add(23, 5.64143503577851);
                    keyValuePairs.Add(20, 5.69241580573566);
                    keyValuePairs.Add(7, 5.75028951497758);
                    keyValuePairs.Add(4, 5.82294756871285);
                }
                else
                    MessageBox.Show("Please select one of R/G/B");
            }
            else if (f1.radioButton_Many_Points.Checked)
            {
             
            }
            else
            {
                MessageBox.Show("Please select one of DataSets");
            }
        }


    }
}
