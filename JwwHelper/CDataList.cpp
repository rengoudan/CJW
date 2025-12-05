#include "pch.h"
#include "CDataList.h"


IMPLEMENT_SERIAL(CDataList, CData, JWW_VERSION | VERSIONABLE_SCHEMA);


CDataList::CDataList() {
	m_nNumber = 0;
	m_bReffered = false;
}

CDataList::~CDataList() {
	/*POSITION pos = m_DataList.GetHeadPosition();
	while (pos != NULL) {
		CData* data = m_DataList.GetNext(pos);
		delete data;
	}*/
	while (!m_DataList.IsEmpty()) {
		delete m_DataList.RemoveHead();
	}
}

void CDataList::Serialize(CArchive& ar)
{
	CData::Serialize(ar);
	if (ar.IsStoring()) {
		ar << m_nNumber;
		ar << m_bReffered;
		ar <<m_time;
		ar << m_strName;
		m_DataList.Serialize(ar);
	}
	else {
		ar >> m_nNumber;
		ar >> m_bReffered;
		ar >> m_time;
		ar >> m_strName;
		m_DataList.Serialize(ar);
	}
}
