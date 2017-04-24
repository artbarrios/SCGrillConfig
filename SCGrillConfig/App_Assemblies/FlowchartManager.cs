using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SCGrillConfig.Models;

namespace SCGrillConfig
{
    #region Flowchart Objects ===============================================
    public class Flowchart
    {
        public string Id { get; set; }
        public List<FlowchartOperator> Operators { get; set; }
        public List<FlowchartLink> Links { get; set; }
        public string ToJSON()
        {
            // converts the flowchart object to a json data string suitable for use as a
            // flowchart javascript data object which can be sent to the view via the ViewBag
            // like this: ViewBag.FlowchartData = myFlowchart.ToJSON();
            // and assigned in the view like this: data: JSON.parse('@ViewBag.FlowchartData'.replace(/&quot;/g, '"')),
            // you have to replace the &quote in the string for it to parse correctly
            string data = "";
            var jsonSerializer = new JavaScriptSerializer();
            // set the default quote character
            char quote = '"';

            // json start
            data += "{";

            // operators start
            data += quote + "operators" + quote + ": {";
            // add operators
            foreach (FlowchartOperator fco in Operators)
            {
                // operator start
                data += quote + fco.Id + quote + ":";
                data += "{";
                data += quote + "top" + quote + ":" + fco.Top.ToString() + ",";
                data += quote + "left" + quote + ":" + fco.Left.ToString() + ",";

                // operator properties start
                data += quote + "properties" + quote + ":";
                data += "{";
                data += quote + "title" + quote + ":" + quote + fco.Title + quote + ",";
                data += quote + "image_src" + quote + ":" + quote + fco.ImageSource + quote + ",";

                // operator inputs start
                data += quote + "inputs" + quote + ":";
                data += "{";
                // add operator inputs
                foreach (FlowchartConnector fcip in fco.Inputs)
                {
                    data += quote + fcip.Id + quote + ":";
                    data += "{" + quote + "label" + quote + ":" + quote + fcip.Label + quote + "},";
                }
                // remove the last comma
                data = data.TrimEnd(',');
                // operator inputs end
                data += "},";

                // operator outputs start
                data += quote + "outputs" + quote + ":";
                data += "{";
                // add operator outputs
                foreach (FlowchartConnector fcop in fco.Outputs)
                {
                    data += quote + fcop.Id + quote + ":";
                    data += "{" + quote + "label" + quote + ":" + quote + fcop.Label + quote + "},";
                }
                // remove the last comma
                data = data.TrimEnd(',');
                // operator outputs end
                data += "}";

                // operator properties end
                data += "},";

                // operatorPopup start
                data += quote + "operatorPopup" + quote + ":{";
                // add popup content
                data += quote + "header" + quote + ":" + jsonSerializer.Serialize(fco.Popup.header) + ",";
                data += quote + "body" + quote + ":" + jsonSerializer.Serialize(fco.Popup.body);
                // operatorPopup end
                data += "}";

                // operator end
                data += "},";
            }
            // remove the last comma
            data = data.TrimEnd(',');

            // operators end
            data += "},";

            // links start
            data += quote + "links" + quote + ":{";
            // add links
            foreach (FlowchartLink fcln in Links)
            {
                // link start
                data += quote + fcln.Id + quote + ":";
                data += "{";
                data += quote + "fromOperator" + quote + ":" + quote + fcln.FromOperatorId + quote + ",";
                data += quote + "fromConnector" + quote + ":" + quote + fcln.FromConnectorId + quote + ",";
                data += quote + "fromSubConnector" + quote + ":" + "0" + ",";
                data += quote + "toOperator" + quote + ":" + quote + fcln.ToOperatorId + quote + ",";
                data += quote + "toConnector" + quote + ":" + quote + fcln.ToConnectorId + quote + ",";
                data += quote + "toSubConnector" + quote + ":" + "0";
                if (fcln.Color.Length > 0)
                {
                    data += "," + quote + "color" + quote + ":" + quote + fcln.Color + quote;
                }
                data += "},";
                // link end
            }
            // remove the last comma
            data = data.TrimEnd(',');

            // links end
            data += "},";

            // operatorTypes start
            data += quote + "operatorTypes" + quote + ":{";
            // operatorTypes end
            data += "},";

            // remove the last comma
            data = data.TrimEnd(',');

            // flowchart end
            data += "}";

            return data;
        }

        public Flowchart()
        {
            Id = "";
            Operators = new List<FlowchartOperator>();
            Links = new List<FlowchartLink>();
        }

    } // public class Flowchart

    public class FlowchartOperator
    {
        public string Id { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public List<FlowchartConnector> Inputs { get; set; }
        public List<FlowchartConnector> Outputs { get; set; }
        public FlowchartPopup Popup { get; set; }
        public FlowchartOperator()
        {
            Id = "";
            Top = 0;
            Left = 0;
            Title = "";
            ImageSource = "";
            Inputs = new List<FlowchartConnector>();
            Outputs = new List<FlowchartConnector>();
            Popup = new FlowchartPopup();
        }
    } // public class FlowchartOperator

    public class FlowchartConnector
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public FlowchartConnector()
        {
            Id = "";
            Label = "";
        }
    } // public class FlowchartConnector 

    public class FlowchartLink
    {
        public string Id { get; set; }
        public string FromOperatorId { get; set; }
        public string FromConnectorId { get; set; }
        public string ToOperatorId { get; set; }
        public string ToConnectorId { get; set; }
        public string Color { get; set; }
        public FlowchartLink()
        {
            Id = "";
            FromOperatorId = "";
            FromConnectorId = "";
            ToOperatorId = "";
            ToConnectorId = "";
            Color = "";
        }
    } // public class FlowchartLink

    public class FlowchartPopup
    {
        public string header { get; set; }
        public string  body { get; set; }
        public FlowchartPopup()
        {
            header = "";
            body = "";
        }
    } // public class FlowchartPopup

    #endregion // Flowchart Objects

    #region FlowchartManager Methods ===============================================
    public class FlowchartManager
    {
        public static Flowchart JSONToFlowchart(string flowchartData)
        {
            // converts a JSON data string containing flowchart data to a Flowchart object
            // data must be valid JSON in flowchart format
            Flowchart flowchart = new Flowchart();
            FlowchartOperator flowchartOperator = null;
            FlowchartConnector flowchartInput = null;
            FlowchartConnector flowchartOutput = null;
            FlowchartLink flowchartLink = null;

            if (flowchartData.Length > 0)
            {
                // get the data into JObject form
                JObject jObject = JObject.Parse(flowchartData);
                // process each operator
                JObject operators = (JObject)jObject["operators"];
                foreach (JProperty myOperator in operators.Properties())
                {
                    // process the operator
                    flowchartOperator = new FlowchartOperator();
                    flowchartOperator.Id = (string)myOperator.Name;
                    JObject level1Properties = (JObject)myOperator.Value;
                    flowchartOperator.Top = (int)level1Properties.GetValue("top");
                    flowchartOperator.Left = (int)level1Properties.GetValue("left");
                    JObject level2Properties = (JObject)level1Properties.GetValue("properties");
                    flowchartOperator.Title = (string)level2Properties.GetValue("title");
                    flowchartOperator.ImageSource = (string)level2Properties.GetValue("image_src");

                    // process each input
                    JObject inputs = (JObject)level2Properties.GetValue("inputs");
                    foreach (JProperty myInput in inputs.Properties())
                    {
                        flowchartInput = new FlowchartConnector();
                        flowchartInput.Id = (string)myInput.Name;
                        JObject inputProperties = (JObject)myInput.Value;
                        flowchartInput.Label = (string)inputProperties.GetValue("label");
                        // add this object
                        flowchartOperator.Inputs.Add(flowchartInput);
                    } // foreach (JProperty myInput

                    // process each output
                    JObject outputs = (JObject)level2Properties.GetValue("outputs");
                    foreach (JProperty myOutput in outputs.Properties())
                    {
                        flowchartOutput = new FlowchartConnector();
                        flowchartOutput.Id = (string)myOutput.Name;
                        JObject outputProperties = (JObject)myOutput.Value;
                        flowchartOutput.Label = (string)outputProperties.GetValue("label");
                        // add this object
                        flowchartOperator.Outputs.Add(flowchartOutput);
                    } // foreach (JProperty myOutput

                    // add this object
                    flowchart.Operators.Add(flowchartOperator);

                } // foreach (JProperty myOperator

                // process each link
                JObject links = (JObject)jObject["links"];
                foreach (JProperty myLink in links.Properties())
                {
                    flowchartLink = new FlowchartLink();
                    flowchartLink.Id = (string)myLink.Name;
                    JObject linkProperties = (JObject)myLink.Value;
                    flowchartLink.FromOperatorId = (string)linkProperties.GetValue("fromOperator");
                    flowchartLink.FromConnectorId = (string)linkProperties.GetValue("fromConnector");
                    flowchartLink.ToOperatorId = (string)linkProperties.GetValue("toOperator");
                    flowchartLink.ToConnectorId = (string)linkProperties.GetValue("toConnector");
                    if (linkProperties.GetValue("color") != null)
                    {
                        flowchartLink.Color = (string)linkProperties.GetValue("color");
                    }

                    // add this object
                    flowchart.Links.Add(flowchartLink);

                } // foreach (JProperty myLink

            } // if (flowchartData.Length > 0)
            return flowchart;

        } // JSONToFlowchart()

    } //  public class FlowchartManager

    #endregion // FlowchartManager Methods
}

