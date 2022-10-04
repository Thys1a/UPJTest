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

		//��ȡ·���µ��ļ�
		XmlDocument document = new XmlDocument();
		document.LoadXml(path);
		if (document == null) return null;
		//�õ�ָ���ڵ��ڵ�����
		XmlNode node = document.SelectSingleNode(name);//.ChildNodes;
		return node;
	}

	/// <summary>
	/// ��ȡ�ýڵ����ӽ������ֺ�ֵ
	/// </summary>
	/// <param name="node"></param>
	/// <returns>����һά�ַ��������list,string[0]=name string[1]=value string[2]=���ڵ�</returns>
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
