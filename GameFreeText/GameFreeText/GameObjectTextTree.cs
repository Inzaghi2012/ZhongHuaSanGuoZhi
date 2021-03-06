﻿namespace GameFreeText
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class GameObjectTextTree
    {
        private Dictionary<string, GameObjectTextBranch> Branches = new Dictionary<string, GameObjectTextBranch>();

        public GameObjectTextBranch GetBranch(string branchName)
        {
            GameObjectTextBranch branch;
            if (this.Branches.TryGetValue(branchName, out branch))
            {
                return branch;
            }
            return null;
        }

        public void LoadFromXmlFile(string filePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            XmlNode nextSibling = document.FirstChild.NextSibling;
            foreach (XmlNode node2 in nextSibling.ChildNodes)
            {
                GameObjectTextBranch branch = new GameObjectTextBranch {
                    BranchName = node2.Attributes.GetNamedItem("Name").Value
                };
                if (node2.Attributes.GetNamedItem("Time") != null)
                {
                    branch.Time = int.Parse(node2.Attributes.GetNamedItem("Time").Value);
                }
                branch.LoadFromXmlNode(node2);
                this.Branches.Add(branch.BranchName, branch);
            }
        }
    }
}

