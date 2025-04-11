# 📄 Document Management System

## Project Description:

This project is a **Document Management System** (DMS) designed to efficiently handle **document storage**, **archiving**, and **retrieval**. The system supports both **file-based storage** (where documents are stored directly on the server) and **database storage** (where documents are stored in binary format in the database).

### 🔑 Key Functionalities:
1. **📤 Document Upload**: 
   - Allows users to upload documents either as files (stored on the server) or in the database (stored in binary format).
   
2. **📦 Archiving**:
   - Automatically archives documents that are outdated or no longer accessed frequently.
   - Documents stored in the database are archived as **zip files**, while server-stored files are kept as-is.

3. **🔍 Document Retrieval**:
   - Enables the retrieval of documents from both the **database** and the **file system**.

4. **📂 Category Management**:
   - Supports categorizing documents to organize them better.

### 🌟 Key Features:
- **🗄️ Two storage formats**: Documents can be stored either on the server (as files) or in the database (in binary format).
- **🗃️ Automatic Archiving**: Outdated documents are automatically archived with zip file creation.
- **🔧 Background Job Processing**: Documents are archived based on the **last access date**.
- **📁 Dynamic Folder Creation**: Folders for file storage are dynamically created based on the document **category**.

The system ensures optimal document management while maintaining a clear separation between server-stored and database-stored files.
