#include "CJwwWriterBase.h"

CJwwWriterBase::CJwwWriterBase() {
}

CJwwWriterBase::~CJwwWriterBase() {
	/*{
		POSITION pos = m_DataList.GetHeadPosition();
		while (pos != NULL) {
			CData* data = m_DataList.GetNext(pos);
			delete data;
		}
	}*/
	/*{
		POSITION pos = m_DataListList.GetHeadPosition();
		while (pos != NULL) {
			CDataList* data = m_DataListList.GetNext(pos);
			delete data;
		}
	}*/
	while (!m_DataList.IsEmpty()) {
		delete m_DataList.RemoveHead();
	}
	while (!m_DataListList.IsEmpty()) {
		delete m_DataListList.RemoveHead();
	}
}

void CJwwWriterBase::Write(LPCTSTR path) {
	try {
		/*AFX_MANAGE_STATE(AfxGetStaticModuleState());
		CFile file;
		file.Open(path, CFile::modeWrite | CFile::modeCreate);
		CArchive ar(&file, CArchive::store);
		WriteFileType(ar);
		WriteHeader(ar);
		WriteData(ar);
		WriteDataList(ar);
		WriteImages(ar);
		ar.Close();*/
		AFX_MANAGE_STATE(AfxGetStaticModuleState());
		CFile file;
		if (!file.Open(path, CFile::modeWrite | CFile::modeCreate)) {
			//AfxThrowFileException(CFileException::fileNotFound);
		}
		CArchive ar(&file, CArchive::store);
		ar.SetObjectSchema(JWW_VERSION);
		CData::s_FileVersion = JWW_VERSION;
		WriteFileType(ar);
		WriteHeader(ar);
		WriteData(ar);
		WriteDataList(ar);
		WriteImages(ar);
		ar.Close();
		file.Close();
	}
	catch (CException* e) {
		TCHAR msg[512];
		e->GetErrorMessage(msg, 512);
		AfxMessageBox(msg);
		e->Delete();
	}
}

void CJwwWriterBase::WriteData(CArchive& ar) {
	for (POSITION pos = m_DataList.GetHeadPosition(); pos != NULL; ) {
		CData* p = m_DataList.GetNext(pos);
		ASSERT(p != nullptr);
		ASSERT_KINDOF(CData, p);
	}
	m_DataList.Serialize(ar);
	/*m_DataList.Serialize(ar);*/
}
void CJwwWriterBase::WriteDataList(CArchive& ar) {
	for (POSITION pos = m_DataListList.GetHeadPosition(); pos != NULL; ) {
		CDataList* p = m_DataListList.GetNext(pos);
		ASSERT(p != nullptr);
		ASSERT_KINDOF(CDataList, p);
	}
	m_DataListList.Serialize(ar);
	/*m_DataListList.Serialize(ar);*/
}

