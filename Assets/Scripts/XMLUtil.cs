using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class XMLUtil 
{
	/// <summary>
	/// 获取该节点下指定子节点
	/// </summary>
	/// <param name="node"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static XmlNode GetSelectedXmlNodeFromRoot(XmlNode node, string name)
	{
		return node.SelectSingleNode(name);
	}

	/// <summary>
	/// 抽取指定xml文件的指定节点
	/// </summary>
	/// <param name="path"></param>
	/// <param name="name">默认为root</param>
	/// <returns>XmlNode</returns>
	public static XmlNode LoadXml(string path, string name = "root")
	{

		//读取路径下的文件
		XmlDocument document = new XmlDocument();
		document.LoadXml(path);
		if (document == null) return null;
		//得到指定节点内的内容
		XmlNode node = document.SelectSingleNode(name);//.ChildNodes;
		return node;
	}

	/// <summary>
	/// 获取该节点下子结点的名字和值
	/// </summary>
	/// <param name="node"></param>
	/// <returns>包含一维字符串数组的list,string[0]=name string[1]=value string[2]=父节点</returns>
	public static List<string[]> GetNameValueFromChildListOfNode(XmlNode node)
	{
		List<string[]> list = new List<string[]>();
		XmlNodeList xmlNodeList = node.ChildNodes;
		foreach (XmlElement e in xmlNodeList)
		{
			string[] stringItem = new string[3];
			stringItem.SetValue(e.Name, 0);
			stringItem.SetValue(e.InnerText, 1);
			stringItem.SetValue(node.Name, 2);
			list.Add(stringItem);
		}
		return list;
	}
	/// <summary>
	/// 获取该节点下特定标签的子节点
	/// </summary>
	/// <param name="node"></param>
	/// <param name="attribute">标签名称</param>
	/// <param name="value">标签的内容</param>
	/// <returns></returns>
	public static XmlElement GetNodeByAttributeFromNode(XmlNode node, string attribute, string value)
	{
		if (node == null) return null;
		XmlNodeList xmlNodeList = node.ChildNodes;

		//Debug.Log(xmlNodeList);
		foreach (XmlElement e in xmlNodeList)
		{
			if (e.Attributes.GetNamedItem(attribute).Value == value) return e;
		}
		return null;
	}

}
