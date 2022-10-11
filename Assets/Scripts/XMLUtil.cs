using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class XMLUtil 
{
	/// <summary>
	/// ��ȡ�ýڵ���ָ���ӽڵ�
	/// </summary>
	/// <param name="node"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static XmlNode GetSelectedXmlNodeFromRoot(XmlNode node, string name)
	{
		return node.SelectSingleNode(name);
	}

	/// <summary>
	/// ��ȡָ��xml�ļ���ָ���ڵ�
	/// </summary>
	/// <param name="path"></param>
	/// <param name="name">Ĭ��Ϊroot</param>
	/// <returns>XmlNode</returns>
	public static XmlNode LoadXml(string path, string name = "root")
	{
		string s = ((Resources.Load(path) as TextAsset)).text;
		//��ȡ·���µ��ļ�
		XmlDocument document = new XmlDocument();
		document.LoadXml(s);
		if (document == null) return null;
		//�õ�ָ���ڵ��ڵ�����
		XmlNode node = document.SelectSingleNode(name);
		return node;
	}

	/// <summary>
	/// ��ȡ�ýڵ�����ֺ�ֵ
	/// </summary>
	/// <param name="node"></param>
	/// <returns>item[0]=name item[1]=value</returns>
	public static string[] GetNameValueOfNode(XmlNode node)
	{
		string[] item = new string[2];
		item.SetValue(node.Name, 0);
		item.SetValue(node.InnerText, 1);
		return item;
	}
	public static List<XmlNode> GetChildNodes(XmlNode node)
    {
		XmlNodeList xmlNodeList = node.ChildNodes;
		List< XmlNode> list = new List<XmlNode>();
		foreach (XmlNode e in xmlNodeList)
		{
			list.Add(e);
		}
		return list;
	}
	/// <summary>
	/// ��ȡ�ýڵ����ض���ǩ���ӽڵ�
	/// </summary>
	/// <param name="node"></param>
	/// <param name="attribute">��ǩ����</param>
	/// <param name="value">��ǩ������</param>
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
