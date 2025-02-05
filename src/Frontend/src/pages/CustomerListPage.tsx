import {
    Box,
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
    styled,
    tableCellClasses,
  } from "@mui/material";
  import { useEffect, useState } from "react";
  
  interface CustomerListQuery {
    id: number;
    name: string;
    address: string;
    email: string;
    phone: string;
    iban: string; 
    categoryCode: string; 
    categoryDescription: string;
  }
  
  export default function CustomerListPage() {
    const [list, setList] = useState<CustomerListQuery[]>([]);

    const [nameFilter, setNameFilter] = useState<string>(""); 
    const [emailFilter, setEmailFilter] = useState<string>("");

  
const fetchData = () => {
      const params = new URLSearchParams();
  
      if (nameFilter) params.append("Name", nameFilter);
      if (emailFilter) params.append("Email", emailFilter);

      fetch(`/api/Customers/list?${params.toString()}`)
        .then((response) =>  response.json())
        .then((data) => {
          setList(data as CustomerListQuery[]);
        });
    };
    
    useEffect(() => {
      fetchData(); 
    }, []);

    const exportToXML = () => {
      const xmlData = `<?xml version="1.0" encoding="UTF-8"?>
    <Customers>
      ${list.map(
        (customer) => `
      <Customer>
        <Id>${customer.id}</Id>
        <Name>${customer.name}</Name>
        <Address>${customer.address}</Address>
        <Email>${customer.email}</Email>
        <Phone>${customer.phone}</Phone>
        <IBAN>${customer.iban}</IBAN>
        <CategoryCode>${customer.categoryCode}</CategoryCode>
        <CategoryDescription>${customer.categoryDescription}</CategoryDescription>
      </Customer>`
      ).join("")}
    </Customers>`;

      const blob = new Blob([xmlData], { type: "application/xml" });
      const url = URL.createObjectURL(blob);

      const a = document.createElement("a");
      a.href = url;
      a.download = "customers.xml";
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    };

    return (
      <>
        <Typography variant="h4" sx={{ textAlign: "center", mt: 4, mb: 4 }}>
          Customers
        </Typography>

        <Box sx={{ display: "flex", gap: 2, mb: 3, justifyContent: "center" }}>
        <TextField
          label="Filter by Name"
          variant="outlined"
          value={nameFilter}
          onChange={(e) => setNameFilter(e.target.value)} 
        />
        <TextField
          label="Filter by Email"
          variant="outlined"
          value={emailFilter}
          onChange={(e) => setEmailFilter(e.target.value)} 
        />
        <Button variant="contained" color="primary" onClick={fetchData}>
          Apply Filters
        </Button>

        <Button variant="contained" color="secondary" onClick={exportToXML}>
          Export to XML
        </Button>
        
        </Box>

        
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 650 }} aria-label="simple table">
            <TableHead>
              <TableRow>
                <StyledTableHeadCell>Name</StyledTableHeadCell>
                <StyledTableHeadCell>Address</StyledTableHeadCell>
                <StyledTableHeadCell>Email</StyledTableHeadCell>
                <StyledTableHeadCell>Phone</StyledTableHeadCell>
                <StyledTableHeadCell>IBAN</StyledTableHeadCell>
                <StyledTableHeadCell>Category Code</StyledTableHeadCell>
                <StyledTableHeadCell>Category Description</StyledTableHeadCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {list.map((row) => (
                <TableRow
                  key={row.id}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
                  <TableCell>{row.name}</TableCell>
                  <TableCell>{row.address}</TableCell>
                  <TableCell>{row.email}</TableCell>
                  <TableCell>{row.phone}</TableCell>
                  <TableCell>{row.iban}</TableCell>
                  <TableCell>{row.categoryCode}</TableCell>
                  <TableCell>{row.categoryDescription}</TableCell> 
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </>
    );
  }
  
  const StyledTableHeadCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
      backgroundColor: theme.palette.primary.light,
      color: theme.palette.common.white,
    },
  }));