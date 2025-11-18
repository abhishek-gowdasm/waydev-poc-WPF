using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        /// <summary>
        /// Get employees by territory
        /// </summary>
        /// <param name="territoryId">Territory ID</param>
        /// <returns>Employees in specified territory</returns>
        [HttpGet("territory/{territoryId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByTerritory(string territoryId)
        {
            var employees = await _employeeService.GetEmployeesByTerritoryAsync(territoryId);
            return Ok(employees);
        }

        /// <summary>
        /// Get employees by region
        /// </summary>
        /// <param name="regionId">Region ID</param>
        /// <returns>Employees in specified region</returns>
        [HttpGet("region/{regionId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByRegion(int regionId)
        {
            var employees = await _employeeService.GetEmployeesByRegionAsync(regionId);
            return Ok(employees);
        }

        /// <summary>
        /// Get employees by hire date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Employees hired in date range</returns>
        [HttpGet("hiredaterange")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByHireDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var employees = await _employeeService.GetEmployeesByHireDateRangeAsync(startDate, endDate);
            return Ok(employees);
        }

        /// <summary>
        /// Get active employees
        /// </summary>
        /// <returns>Currently active employees</returns>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetActiveEmployees()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Get employees by title
        /// </summary>
        /// <param name="title">Job title</param>
        /// <returns>Employees with job title</returns>
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByTitle(string title)
        {
            var employees = await _employeeService.GetEmployeesByTitleAsync(title);
            return Ok(employees);
        }

        /// <summary>
        /// Search employees
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Matching employees</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees([FromQuery] string searchTerm)
        {
            var employees = await _employeeService.SearchEmployeesAsync(searchTerm);
            return Ok(employees);
        }

        /// <summary>
        /// Get top employees
        /// </summary>
        /// <param name="count">Number of employees to return</param>
        /// <returns>Top employees by sales</returns>
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetTopEmployees(int count)
        {
            var employees = await _employeeService.GetTopEmployeesAsync(count);
            return Ok(employees);
        }

        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="createEmployeeDto">Employee data</param>
        /// <returns>Created employee</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { error = "An error occurred while creating the employee. Please try again." });
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="updateEmployeeDto">Updated employee data</param>
        /// <returns>Updated employee</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
                return Ok(employee);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        /// <returns>Employee statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<EmployeeStatisticsDto>> GetEmployeeStatistics()
        {
            var statistics = await _employeeService.GetEmployeeStatisticsAsync();
            return Ok(statistics);
        }

        /// <summary>
        /// Get employee hierarchy
        /// </summary>
        /// <returns>Employee hierarchy structure</returns>
        [HttpGet("hierarchy")]
        public async Task<ActionResult<IEnumerable<EmployeeHierarchyDto>>> GetEmployeeHierarchy()
        {
            var hierarchy = await _employeeService.GetEmployeeHierarchyAsync();
            return Ok(hierarchy);
        }
    }
} 